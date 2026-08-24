using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Models;
using OnlineShop.DB.Models.Users;
using OnlineShopWebApp.Areas.Admin.Models;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var users = _userManager.Users.ToListAsync().Result;
            var userRoles = _roleManager.Roles.ToListAsync().Result;

            var usersVM = new List<UserViewModel>();

            foreach (var user in users)
            {
                var roles = _userManager.GetRolesAsync(user).Result;
                var role = roles.FirstOrDefault() ?? Constants.UserRoleName;
                usersVM.Add(user.ToUserViewModel(role));
            }

            var tuple = Tuple.Create(usersVM, userRoles.ToRoleViewModels());
            return View(tuple);
        }

        public IActionResult Update(string userID)
        {
            var user = _userManager.FindByIdAsync(userID).Result;
            var roles = _userManager.GetRolesAsync(user).Result;
            var role = roles.FirstOrDefault();

            return View(user.ToUserViewModel(role));
        }

        [HttpPost]
        public IActionResult Update(UserViewModel user)
        {
            var existingUser = _userManager.FindByIdAsync(user.ID).Result;
            var userWithSameLogin = _userManager.FindByNameAsync(user.Login).Result;

            if (userWithSameLogin != null && userWithSameLogin.Id != existingUser.Id)
            {
                ModelState.AddModelError("Login", "Пользователь с таким логином уже существует!");
            }

            var userWithSamePhone = _userManager.Users.FirstOrDefault(u => u.PhoneNumber == user.PhoneNumber);

            if (userWithSamePhone != null && userWithSamePhone.Id != user.ID)
            {
                ModelState.AddModelError("PhoneNumber", "Этот номер телефона уже используется!");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            existingUser.UserName = user.Login;
            existingUser.Email = user.Login;
            existingUser.FirstName = user.FirstName;
            existingUser.LastName = user.LastName;
            existingUser.Patronymic = user.Patronymic;
            existingUser.PhoneNumber = user.PhoneNumber;

            var result = _userManager.UpdateAsync(existingUser).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);                   
                }
                return View(user);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ChangePassword(string userId)
        {
            var user = _userManager.FindByIdAsync(userId).Result;

            var changePassword = new ChangePasswordViewModel
            {
                userID = user.Id,
                Login = user.UserName
            };

            return View(changePassword);
        }

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel changePassword)
        {
            if (changePassword.Login == changePassword.Password)
            {
                ModelState.AddModelError("Password", "Логин и пароль не должны совпадать!");
            }

            if (!ModelState.IsValid)
            {
                return View(changePassword);
            }

            var user = _userManager.FindByIdAsync(changePassword.userID).Result;
            var removeResult = _userManager.RemovePasswordAsync(user).Result;

            if (removeResult.Succeeded || !_userManager.HasPasswordAsync(user).Result)
            {
                var result = _userManager.AddPasswordAsync(user, changePassword.Password).Result;
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                    return View(changePassword);
                }                
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult ChangeRole(string userID, string roleName)
        {
            var user = _userManager.FindByIdAsync(userID).Result;
            var currentRoles = _userManager.GetRolesAsync(user).Result;

            var removeRolesResult = _userManager.RemoveFromRolesAsync(user, currentRoles).Result;
            if (!removeRolesResult.Succeeded)
            {
                foreach (var error in removeRolesResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            var result = _userManager.AddToRoleAsync(user, roleName).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(UserViewModel user)
        {
            var existingUser = _userManager.FindByNameAsync(user.Login).Result;

            if (existingUser != null)
            {
                ModelState.AddModelError("Login", "Такой пользователь уже существует!");
            }

            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var newUser = new User
            {
                UserName = user.Login,
                Email = user.Login,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Patronymic = user.Patronymic,
                PhoneNumber = user.PhoneNumber,
                RegistrationDateTime = DateTime.UtcNow,
                IsCancelled = false
            };

            var result = _userManager.CreateAsync(newUser).Result;
            if (!result.Succeeded)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(user);
                }
            }

            var addRoleResult = _userManager.AddToRoleAsync(newUser, Constants.UserRoleName).Result;
            if (!addRoleResult.Succeeded)
            {
                foreach (var error in addRoleResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                    return View(user);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(string userID)
        {
            var user = _userManager.FindByIdAsync(userID).Result;

            user.IsCancelled = true;
            _userManager.UpdateAsync(user).Wait();

            return RedirectToAction(nameof(Index));
        }
    }
}
