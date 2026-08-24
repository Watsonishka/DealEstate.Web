using OnlineShop.DB.Models; 

namespace OnlineShopWebApp.Models
{
    public class BidViewModel
    {
        public readonly string ManagerPhoneNumber = "+7 800-555-35-35";
        public Guid ID { get; set; }
        public string UserID { get; set; }
        public List<BidItemViewModel> Items { get; set; } 
        public decimal ProductsTotalPrice => Items?.Sum(x => x.Product?.Cost ?? 0) ?? 0;
        public decimal ServicesTotalPrice => Items?.Sum(x => x.Services?.Where(s => !s.IsRemoved).Sum(s => s.Price) ?? 0) ?? 0;
        public decimal ManagerReward => totalPrice * Constants.CommissionPercent;
        private decimal totalPrice => ProductsTotalPrice + ServicesTotalPrice;
    }
}
