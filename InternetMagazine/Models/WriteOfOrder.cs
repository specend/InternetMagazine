using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class WriteOfOrder
    {
        [Key]
        public int Id_record { get; set; }

        public int Id_customer { get; set; }
        public string IdentityNumber { get; set; }
        public DateTime DateOfOrder { get; set; }

        [Required(ErrorMessage = "Введите ваш адрес")]
        [Display(Name = "Адрес")]
        public string Address { get; set; }

        public string StateOrder { get; set; }

        [Required(ErrorMessage = "Укажите тип оплаты")]
        [Display(Name = "Тип оплаты")]
        public string TypeOfPay { get; set; }

        [Required(ErrorMessage = "Укажите тип доставки")]
        [Display(Name = "Тип доставки")]
        public string TypeOfDelivery { get; set; }

        public int AdditionalCharges { get; set; }

        public Customer Customer { get; set; }

        public virtual List<Order> Orders { get; set; }
    }
}
