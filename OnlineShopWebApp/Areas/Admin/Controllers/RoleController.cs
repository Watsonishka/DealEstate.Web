using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OnlineShop.DB.Models;
using OnlineShopWebApp.Helpers;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var roles = _roleManager.Roles.ToListAsync().Result;
            return View(roles.ToRoleViewModels());
        }

        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(RoleViewModel role)
        {
            var existingRole = _roleManager.FindByNameAsync(role.Name).Result;

            if (existingRole != null)
            {
                ModelState.AddModelError("", "Такая роль уже существует!");
            }

            if (!ModelState.IsValid)
            {
                return View(role);
            }

            var createResult = _roleManager.CreateAsync(new IdentityRole(role.Name)).Result;
            if (!createResult.Succeeded)
            {
                foreach (var error in createResult.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public IActionResult Delete(string roleID)
        {
            var role = _roleManager.FindByIdAsync(roleID).Result;

            if (role != null)
            {
                _roleManager.DeleteAsync(role).Wait();
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
