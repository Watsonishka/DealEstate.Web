using System.ComponentModel.DataAnnotations;

namespace OnlineShop.DB.Models.Account
{
    public class RegistrationViewModel
    {
        [DataType(DataType.EmailAddress)]
        [Display(Name = "Укажите Ваш логин")]
        [EmailAddress(ErrorMessage = "Введите валидный email")]
        [Required(ErrorMessage = "Не указан email! Это является обязательным полем!")]
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
        public string ConfirmPassword { get; set; }

        [Display(Name = "Укажите Вашу фамилию")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Фамилия должна состоять минимум из 2 и не более 50 символов!")]
        [RegularExpression(@"^[А-ЯЁа-яё\s\-']+$", ErrorMessage = "Фамилия должна содержать только русские буквы, пробел, дефис и апостроф")]
        public string? LastName { get; set; }

        [Display(Name = "Укажите Ваше имя")]
        [Required(ErrorMessage = "Не введено имя! Это является обязательным полем!")]
        [StringLength(25, MinimumLength = 2, ErrorMessage = "Имя должно состоять минимум из 2 и не более 25 символов!")]
        [RegularExpression(@"^[А-ЯЁа-яё\s\-']+$", ErrorMessage = "Имя должно содержать только русские буквы, пробел, дефис и апостроф")]
        public string FirstName { get; set; }

        [Display(Name = "Укажите Ваше отчество")]
        [StringLength(50, ErrorMessage = "Отчество не может превышать более 50 символов!")]
        [RegularExpression(@"^[А-ЯЁа-яё\s\-']+$", ErrorMessage = "Отчество должно содержать только русские буквы, пробел, дефис и апостроф")]
        public string? Patronymic { get; set; }

        [DataType(DataType.PhoneNumber)]
        [Display(Name = "Укажите Ваш номер телефона в формате +79000000000")]
        [Required(ErrorMessage = "Не указан номер телефона! Это является обязательным полем!")]
        [StringLength(16, MinimumLength = 5, ErrorMessage = "Номер телефонв должен состоять минимум из 5 и не более 16 цифр!")]
        [RegularExpression(@"^[0-9\+]+$", ErrorMessage = "Номер телефона должен содержать только цифры и знак +")]
        public string PhoneNumber { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
