using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class Customer
    {
        [Key]
        public int Id_Customer { get; set; }

        [Required(ErrorMessage = "Введите ваше ФИО")]
        [Display(Name = "ФИО")]
        public string FIO { get; set; }

        [Required(ErrorMessage = "Укажите ваш пол")]
        [Display(Name = "Пол")]
        public char Gender { get; set; }

        [Required(ErrorMessage = "Введите дату рождения")]
        [DataType(DataType.Date)]
        [Display(Name = "Дата рождения")]
        public DateTime DateOfBirth { get; set; }

        [Required(ErrorMessage = "Введите ваш адрес электронной почты")]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Введите корректный адрес электронной почты")]
        [Display(Name = "Электронная почта")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Введите ваш номер телефона")]
        //[RegularExpression("+79+d[0-9]{9}", ErrorMessage = "Введите корректный номер телефона")]
        [Display(Name = "Номер телефона")]
        public string Phone { get; set; }

        [Required(ErrorMessage = "Введите ваш логин")]
        [Display(Name = "Логин")]
        public string Login { get; set; }

        [Required(ErrorMessage = "Введите ваш пароль")]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Подтвердите ваш пароль")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Пароли не совпадают")]
        [StringLength(100, ErrorMessage = "Пароль должен содержать как минимум 6 символов", MinimumLength = 6)]
        [Display(Name = "Подтверждение пароля")]
        public string ConfirmPassword { get; set; }

        public bool RememberMe { get; set; }

        public List<Basket> Baskets { get; set; }
        public List<WriteOfOrder> WriteOfOrders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
