using System.ComponentModel;

namespace OnlineShop.DB.Models.Orders
{
    public enum OrderStatus
    {
        [Description("Создан")]
        Created,

        [Description("В обработке")]
        InProgress,

        [Description("Завершён")]
        Completed
    }
}