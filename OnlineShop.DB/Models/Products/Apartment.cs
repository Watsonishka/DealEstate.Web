namespace OnlineShop.DB.Models.Products
{
    public class Apartment : Product
    {
        public int Floor { get; set; }
        public bool HasBalcony { get; set; }
        public double CeilingHeight { get; set; }
    }
}