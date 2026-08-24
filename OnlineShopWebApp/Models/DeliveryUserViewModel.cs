using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class DeliveryUserViewModel
    {
        public Guid ID { get; set; }

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

        [Display(Name = "Оставьте Ваш комментарий")]
        [StringLength(512, ErrorMessage = "Комментарий не может превышать более 512 символов!")]
        public string? Comment { get; set; }
    }
}
