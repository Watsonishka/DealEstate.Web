namespace OnlineShop.DB.Models.Orders
{
    public class DeliveryUser
    {
        public Guid ID { get; set; }
        public string? LastName { get; set; }
        public string FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string PhoneNumber { get; set; }
        public string? Comment { get; set; }
        public List<Order> Orders { get; set; }
    }
}