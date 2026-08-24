using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Areas.Admin.Models
{
    public class ChangePasswordViewModel
    {
        public string userID { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Укажите Ваш логин")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        [Required(ErrorMessage = "Не указан логин! Это является обязательным полем!")]
        [StringLength(30, MinimumLength = 5, ErrorMessage = "Логин должен состоять минимум из 5 и не более 30 символов!")]
        public string Login { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Укажите Ваш пароль")]
        [Required(ErrorMessage = "Не указан пароль! Это является обязательным полем!")]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Пароль должен состоять минимум из 6 и не более 50 символов!")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Подтверждение пароля")]
        [Required(ErrorMessage = "Не указан повторный пароль")]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        public string? ConfirmPassword { get; set; }

    }
}
