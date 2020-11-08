using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InternetMagazine.Models
{
    public class OrderViewModel
    {
        [Required(ErrorMessage = "Введите ваш адрес")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        [Required(ErrorMessage = "Укажите тип оплаты")]
        [Display(Name = "Тип оплаты")]
        public string TypeOfPay { get; set; }

        [Required(ErrorMessage = "Укажите тип доставки")]
        [Display(Name = "Тип доставки")]
        public string TypeOfDelivery { get; set; }
    }
}
