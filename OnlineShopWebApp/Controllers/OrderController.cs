using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using OnlineShop.DB.Interfaces;
using OnlineShop.DB.Models.Bids;
using OnlineShop.DB.Models.Orders;
using OnlineShop.DB.Models.Users;
using OnlineShopWebApp.Helpers;
using OnlineShopWebApp.Models;

namespace OnlineShopWebApp.Controllers
{
    [Authorize]
    public class OrderController : Controller
    {
        private readonly IBidsStorage _bidsStorage;
        private readonly IOrdersStorage _ordersStorage;
        private readonly UserManager<User> _userManager;
        public OrderController(IBidsStorage bidsStorage, IOrdersStorage ordersStorage, UserManager<User> userManager)
        {
            _bidsStorage = bidsStorage;
            _ordersStorage = ordersStorage;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var currentUserID = _userManager.GetUserId(User);
            var orders = _ordersStorage.TryGetAllByUserId(currentUserID);
            return View(orders.ToOrdersViewModels());
        }

        public IActionResult Registrate()
        {
            var currentUserID = _userManager.GetUserId(User);
            var bid = _bidsStorage.TryGetByUserId(currentUserID);
            return View(bid.ToBidViewModel());
        }

        [HttpPost]
        public IActionResult Send(DeliveryUserViewModel user)
        {
            var currentUserID = _userManager.GetUserId(User);
            var bid = _bidsStorage.TryGetByUserId(currentUserID);

            if (!ModelState.IsValid)
            {
                ViewBag.DeliveryUserViewModel = user;
                return View("Registrate", bid.ToBidViewModel());
            }
            var bidDB = _bidsStorage.TryGetByUserId(currentUserID);

            var order = new Order
            {
                UserID = bidDB.UserID,
                DeliveryUser = user.ToDeliveryUserDb(),
                CreatedAt = DateTime.UtcNow,
                Status = OrderStatus.Created,
                Items = bidDB.Items?.Select(item => new OrderItem
                {
                    ProductID = item.Product.ID,
                    ProductName = item.Product.Name,
                    ProductCity = item.Product.City,
                    ProductArea = item.Product.Area,
                    CurrentPrice = item.Product.Cost,
                    Services = item.Services?.Select(service => new Service
                    {
                        Name = service.Name,
                        Price = service.Price,
                        IsRemoved = service.IsRemoved
                    }).ToList()
                }).ToList()
            };

            _ordersStorage.Add(order);
            _bidsStorage.Clear(currentUserID);

            return RedirectToAction("Success");
        }

        public IActionResult Success()
        {
            return View();
        }
    }
}
