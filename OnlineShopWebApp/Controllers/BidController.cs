using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Interfaces;

namespace OnlineShopWebApp.Controllers
{
    public class BidController : Controller
    {
        private readonly IProductsStorage _productsStorage;
        private readonly IBidsStorage _bidsStorage;
        private readonly IUserContextService _userContextService;

        public BidController(IProductsStorage productsStorage, IBidsStorage bidsStorage, IUserContextService userContextService)
        {
            _productsStorage = productsStorage;
            _bidsStorage = bidsStorage;
            _userContextService = userContextService;
        }
        public IActionResult Index()
        {
            var userId = _userContextService.GetCurrentUserID();
            var anonymousId = _userContextService.GetAnonymousID();
            var bid = _bidsStorage.TryGetByUserOrAnonymous(userId, anonymousId);

            return View(bid?.ToBidViewModel());
        }

        public IActionResult Add(Guid productId)
        {
            var product = _productsStorage.TryGetByID(productId);
            var currentUserID = _userContextService.GetCurrentUserID();
            var anonymousID = _userContextService.GetAnonymousID();

            if (product != null)
            {
                _bidsStorage.Add(product, currentUserID, anonymousID);
            }

            return RedirectToAction("Index", "Home");
        }

        public IActionResult Remove(Guid productId)
        {
            var currentUserID = _userContextService.GetCurrentUserID();
            var anonymousID = _userContextService.GetAnonymousID();
            var product = _productsStorage.TryGetByID(productId);

            if (product != null)
            {
                _bidsStorage.Remove(product, currentUserID, anonymousID);
            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult ToggleService(Guid serviceId)
        {
            _bidsStorage.ToggleService(serviceId);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Clear()
        {
            var currentUserID = _userContextService.GetCurrentUserID();

            if (currentUserID == null)
            {
                currentUserID = _userContextService.GetAnonymousID();
            }

            _bidsStorage.Clear(currentUserID);
            return RedirectToAction(nameof(Index));
        }
    }
}
