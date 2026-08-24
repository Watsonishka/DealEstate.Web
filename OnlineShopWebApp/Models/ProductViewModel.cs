using OnlineShop.DB.Models.Products;
using System.ComponentModel.DataAnnotations;

namespace OnlineShopWebApp.Models
{
    public class ProductViewModel
    {
        public Guid ID { get; set; }

        [Display(Name = "Укажите наименование объекта недвижимости")]
        [Required(ErrorMessage = "Не указано наименование объекта недвижимости! Это является обязательным полем!")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Наименование объекта недвижимости должно состоять минимум из 2 и не более 200 символов!")]
        public string Name { get; set; }

        [Display(Name = "Укажите стоимость объекта недвижимости")]
        [Required(ErrorMessage = "Не указана стоимость объекта недвижимости! Это является обязательным полем!")]
        [Range(1, 1000000000, ErrorMessage = "Стоимость объекта недвижимости должна быть от 1 до 1 000 000 000 рублей")]
        public decimal Cost { get; set; }

        [Display(Name = "Укажите площадь объекта недвижимости")]
        [Required(ErrorMessage = "Не указана площадь объекта недвижимости! Это является обязательным полем!")]
        [Range(1, 10000, ErrorMessage = "Площадь должна быть больше 0 м²!")]
        public double Area { get; set; }

        [Display(Name = "Укажите описание объекта недвижимости")]
        [StringLength(4096, ErrorMessage = "Описание не может превышать более 4096 символов!")]
        public string? Description { get; set; }

        [Display(Name = "Укажите этажность объекта недвижимости")]
        [Required(ErrorMessage = "Не указана этажность объекта недвижимости! Это является обязательным полем!")]
        [Range(1, 100, ErrorMessage = "Этажность должна быть не менее 1 и не более 100!")]
        public int TotalFloors { get; set; }

        public Category Category { get; set; }

        [Display(Name = "Укажите застройщика объекта недвижимости")]
        [Required(ErrorMessage = "Не указан застройщик объекта недвижимости! Это является обязательным полем!")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Наименование застройщика должно состоять минимум из 2 и не более 200 символов!")]
        public string Developer { get; set; }

        [Display(Name = "Укажите город объекта недвижимости")]
        [Required(ErrorMessage = "Не указан город объекта недвижимости! Это является обязательным полем!")]
        [StringLength(200, MinimumLength = 2, ErrorMessage = "Название города должно состоять минимум из 2 и не более 200 символов!")]
        [RegularExpression(@"^[А-ЯЁа-яё\s\-]+$", ErrorMessage = "Название города должно содержать только русские буквы, пробел и дефис")]
        public string City { get; set; }       

        public string? PreviewImagePath { get; set; }

        [Display(Name = "Загрузите изображение")]
        public IFormFile? UploadedImage { get; set; }

        [Display(Name = "Укажите класс жилья")]
        public ApartmentClass Class { get; set; }

        [Display(Name = "Укажите этаж квартиры")]
        [Required(ErrorMessage = "Не указан этаж квартиры! Это является обязательным полем!")]
        [Range(1, 100, ErrorMessage = "Этажность должна быть не менее 1 и не более 100!")]
        [FloorRange]
        public int Floor { get; set; }
        public bool HasBalcony { get; set; }

        [Display(Name = "Укажите высоту потолка квартиры")]
        [Required(ErrorMessage = "Не указана высота потолка квартиры! Это является обязательным полем!")]
        [Range(2, 10, ErrorMessage = "Высота потолка должна быть не менее 2 и не более 10!")]
        public double CeilingHeight { get; set; }

        [Display(Name = "Укажите площадь земельного участка")]
        [Required(ErrorMessage = "Не указана площадь земельного участка! Это является обязательным полем!")]
        [Range(0, 10000, ErrorMessage = "Значение должно быть не менее 0 (0 указывается в случае отсутствия земельного участка!)")]
        public double LandArea { get; set; }
        public bool HasGarage { get; set; }
    }
}
