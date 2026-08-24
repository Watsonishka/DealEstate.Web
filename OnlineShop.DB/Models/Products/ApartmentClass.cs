using System.ComponentModel;

namespace OnlineShop.DB.Models.Products
{
    public enum ApartmentClass
    {
        [Description("Эконом")]
        Economy,

        [Description("Комфорт")]
        Comfort,

        [Description("Бизнес")]
        Business,

        [Description("Премиум")]
        Premium,

        [Description("Элитная")]
        Elite
    }
}
