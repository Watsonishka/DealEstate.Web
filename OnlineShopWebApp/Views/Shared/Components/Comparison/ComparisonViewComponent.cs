using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Users;

namespace OnlineShopWebApp.Views.Shared.Components.Comparison
{
    public class ComparisonViewComponent : ViewComponent
    {
        private readonly IComparisonsStorage _comparisonsStorage;
        private readonly UserManager<User> _userManager;
        public ComparisonViewComponent(IComparisonsStorage comparisonsStorage, UserManager<User> userManager)
        {
            _userManager = userManager;
            _comparisonsStorage = comparisonsStorage;
        }

        public IViewComponentResult Invoke()
        {
            var currentUserID = _userManager.GetUserId(UserClaimsPrincipal);

            var comparison = _comparisonsStorage.TryGetByUserID(currentUserID);
            var productsCount = comparison?.Products.Count ?? 0;

            return View("Comparison", productsCount);
        }
    }
}