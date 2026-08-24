using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Users;

namespace OnlineShopWebApp.Views.Shared.Components.Bid
{
    public class BidViewComponent : ViewComponent
    {
        private readonly IBidsStorage _bidsStorage;
        private readonly UserManager<User> _userManager;

        public BidViewComponent(IBidsStorage bidsStorage, UserManager<User> userManager)
        {
            _bidsStorage = bidsStorage;
            _userManager = userManager;
        }

        public IViewComponentResult Invoke()
        {
            var currentUserID = _userManager.GetUserId(UserClaimsPrincipal);

            var bid = _bidsStorage.TryGetByUserId(currentUserID);
            var productsCount = bid?.Items.Count ?? 0;

            return View("Bid", productsCount);
        }
    }
}
