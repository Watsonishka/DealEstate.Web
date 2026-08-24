using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Orders;
using OnlineShopWebApp.Helpers;
using OnlineShop.DB.Models;

namespace OnlineShopWebApp.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = Constants.AdminRoleName)]
    public class OrderController : Controller
    {
        private readonly IOrdersStorage _ordersStorage;
        public OrderController(IOrdersStorage ordersStorage)
        {
            _ordersStorage = ordersStorage;
        }
        public IActionResult Index()
        {
            var orders = _ordersStorage.GetAll();
            return View(orders.ToOrdersViewModels());
        }
        public IActionResult Show(Guid orderId)
        {
            var order = _ordersStorage.TryGetById(orderId);
            return View(order.ToOrderViewModel());
        }

        [HttpPost]
        public IActionResult UpdateStatus(Guid orderID, OrderStatus status)
        {
            _ordersStorage.UpdateStatus(orderID, status);
            return RedirectToAction(nameof(Index));
        }
    }
}
