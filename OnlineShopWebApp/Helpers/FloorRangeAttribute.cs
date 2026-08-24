using OnlineShop.DB.Models.Products;
using System.ComponentModel.DataAnnotations;
using OnlineShopWebApp.Models;

public class FloorRangeAttribute : ValidationAttribute
{
    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var floorNumber = (int)value;
        var currentObject = validationContext.ObjectInstance;
        int totalFloors = 0;

        if (currentObject is ProductViewModel product)
        {
            totalFloors = product.TotalFloors;
        }
        if (currentObject is Apartment apartment)
        {
            totalFloors = apartment.TotalFloors;
        }
        if (currentObject is House house)
        {
            return ValidationResult.Success;
        }


        if (floorNumber > totalFloors)
        {
            return new ValidationResult("Номер этажа не может быть больше общего количества этажей!");
        }

        return ValidationResult.Success;
    }
}