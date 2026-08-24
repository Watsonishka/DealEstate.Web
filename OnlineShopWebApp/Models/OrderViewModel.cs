using OnlineShop.DB.Models.Orders;
using OnlineShop.DB.Models;

namespace OnlineShopWebApp.Models
{
    public class OrderViewModel
    {
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = [];
        public DeliveryUserViewModel DeliveryUser { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public decimal ProductsTotalPrice => Items?.Sum(x => x.CurrentPrice) ?? 0;
        public decimal ServicesTotalPrice => Items?.Sum(x => x.Services?.Where(s => !s.IsRemoved).Sum(s => s.Price) ?? 0) ?? 0;
        public decimal ManagerReward => totalPrice * Constants.CommissionPercent;
        private decimal totalPrice => ProductsTotalPrice + ServicesTotalPrice;
    }
}

