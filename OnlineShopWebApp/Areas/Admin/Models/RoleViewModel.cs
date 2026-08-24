using System.ComponentModel.DataAnnotations;
namespace OnlineShopWebApp.Areas.Admin;

public class RoleViewModel
{
    public string? ID { get; set; }

    [Display(Name = "Название роли")]
    [Required(ErrorMessage = "Не указана роль")]
    [StringLength(50, MinimumLength = 2, ErrorMessage = "Название роли должно быть от 2 до 50 символов!")]
    public string Name { get; set; }

    public bool IsCustom { get; set; }
}
