using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models;
using OnlineShop.DB.Models.Account;
using OnlineShop.DB.Models.Users;

namespace OnlineShopWebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IBidsStorage _bidsStorage;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IBidsStorage bidsStorage)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _bidsStorage = bidsStorage;
        }

        public IActionResult Authorization(string returnUrl = "/")
        {
            return View(new AuthorizationViewModel() { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Authorization(AuthorizationViewModel authorization)
        {
            if (!ModelState.IsValid)
            {
                return View(authorization);
            }

            var anonymousId = HttpContext.Items["AnonymousID"]?.ToString();
            var user = _userManager.FindByNameAsync(authorization.Login).Result;

            if (user == null)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(authorization);
            }

            if (user.IsCancelled)
            {
                var isPasswordValid = _userManager.CheckPasswordAsync(user, authorization.Password).Result;
                if (isPasswordValid)
                {
                    ModelState.AddModelError("", "Ваш аккаунт был удалён. Вы можете восстановить аккаунт, зарегистрировавшись заново");
                }
                else
                {
                    ModelState.AddModelError("", "Неверный логин или пароль");
                }

                return View(authorization);
            }

            var result = _signInManager.PasswordSignInAsync(
                authorization.Login,
                authorization.Password,
                authorization.IsRememberMe,
                false).Result;

            if (!result.Succeeded)
            {
                ModelState.AddModelError("", "Неверный логин или пароль");
                return View(authorization);
            }

            if (!string.IsNullOrEmpty(anonymousId))
            {
                _bidsStorage.Merge(anonymousId, user.Id);
            }

            return Redirect(authorization.ReturnUrl ?? "/");
        }

        public IActionResult Registration(string returnUrl = "/")
        {
            return View(new RegistrationViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        public IActionResult Registration(RegistrationViewModel registration)
        {
            var anonymousId = HttpContext.Items["AnonymousID"]?.ToString();
            var existingUser = _userManager.FindByNameAsync(registration.Login).Result;

            if (registration.Login == registration.Password)
            {
                ModelState.AddModelError("", "Имя и пароль не должны совпадать");
            }

            if (existingUser != null && !existingUser.IsCancelled)
            {
                ModelState.AddModelError("", "Пользователь с таким логином уже зарегистрирован!\r\n" +
                    "Необходимо зарегистрироваться под другим логином!");
            }

            var existingPhoneUser = _userManager.Users
                .FirstOrDefaultAsync(x =>
                    x.PhoneNumber == registration.PhoneNumber &&
                    !x.IsCancelled).Result;

            if (existingPhoneUser != null && existingPhoneUser.Id != existingUser?.Id)
            {
                ModelState.AddModelError("", "Пользователь с таким номером телефона уже зарегистрирован!\r\n" +
                    "Необходимо зарегистрироваться с другим номером телефона!");
            }

            if (!ModelState.IsValid)
            {
                return View(registration);
            }

            var userToAuth = new User();

            if (existingUser != null && existingUser.IsCancelled)
            {
                existingUser.IsCancelled = false;
                existingUser.FirstName = registration.FirstName;
                existingUser.LastName = registration.LastName;
                existingUser.Patronymic = registration.Patronymic;
                existingUser.PhoneNumber = registration.PhoneNumber;
                existingUser.PasswordHash =
                    _userManager.PasswordHasher.HashPassword(
                        existingUser,
                        registration.Password);

                var updateResult = _userManager.UpdateAsync(existingUser).Result;

                if (!updateResult.Succeeded)
                {
                    return View(registration);
                }

                userToAuth = existingUser;
            }
            else
            {
                var newUser = new User
                {
                    UserName = registration.Login,
                    Email = registration.Login,
                    FirstName = registration.FirstName,
                    LastName = registration.LastName,
                    Patronymic = registration.Patronymic,
                    PhoneNumber = registration.PhoneNumber,
                    RegistrationDateTime = DateTime.UtcNow,
                    IsCancelled = false
                };

                var createResult = _userManager.CreateAsync(newUser, registration.Password).Result;

                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(registration);
                }

                var addRoleResult = _userManager.AddToRoleAsync(newUser, Constants.UserRoleName).Result;

                if (!addRoleResult.Succeeded)
                {
                    foreach (var error in addRoleResult.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }

                    return View(registration);
                }

                userToAuth = newUser;
            }

            _signInManager.SignInAsync(userToAuth, true).Wait();

            if (!string.IsNullOrEmpty(anonymousId))
            {
                _bidsStorage.Merge(anonymousId, userToAuth.Id);
            }

            return Redirect(registration.ReturnUrl ?? "/");
        }

        public IActionResult Logout()
        {
            _signInManager.SignOutAsync().Wait();

            return RedirectToAction(nameof(Index), "Home");
        }
    }
}