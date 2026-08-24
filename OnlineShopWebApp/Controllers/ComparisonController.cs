using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Users;
using OnlineShopWebApp.Helpers;


namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class ComparisonController : Controller
    {
        private readonly IComparisonsStorage _comparisonsStorage;
        private readonly IProductsStorage _productsStorage;
        private readonly UserManager<User> _userManager;

        public ComparisonController(IComparisonsStorage comparisonsStorage, IProductsStorage productsStorage, UserManager<User> userManager)
        {
            _comparisonsStorage = comparisonsStorage;
            _productsStorage = productsStorage;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUserID = _userManager.GetUserId(User);
            var comparison = _comparisonsStorage.TryGetByUserID(currentUserID);
            return View(comparison.ToComparsionViewModel());
        }

        public IActionResult Add(Guid productID)
        {
            var currentUserID = _userManager.GetUserId(User);
            var product = _productsStorage.TryGetByID(productID);

            if (product != null)
            {
                _comparisonsStorage.Add(product, currentUserID);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove(Guid productID)
        {
            var currentUserID = _userManager.GetUserId(User);
            _comparisonsStorage.Remove(productID, currentUserID);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            var currentUserID = _userManager.GetUserId(User);
            _comparisonsStorage.Clear(currentUserID);
            return RedirectToAction(nameof(Index));
        }
    }
}