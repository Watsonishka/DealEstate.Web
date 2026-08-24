namespace OnlineShopWebApp.Models
{
    public class ServiceViewModel
    {
        public Guid ID { get; set; }
        public bool IsRemoved { get; set; } 
        public decimal Price { get; set; }
        public string Name { get; set; }
    }
}
