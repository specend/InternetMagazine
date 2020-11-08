using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class ValidateViewModel
    {

        [Required(ErrorMessage = "Введите ваш адрес электронной почты")]
        [EmailAddress]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введите корректный адрес электронной почты")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

    }
}
