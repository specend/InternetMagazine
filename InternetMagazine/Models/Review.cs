using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class Review
    {
        [Key]
        public int Id_Review { get; set; }
        public int Id_Customer { get; set; }
        public int Id_product { get; set; }

        [Required(ErrorMessage = "Заполните поле")]
        [Display(Name = "Отзыв")]
        public string Comment { get; set; }
        public DateTime DateOfWrite { get; set; }

        [Required(ErrorMessage = "Поставьте оценку")]
        [Display(Name = "Оценка")]
        public int Mark { get; set; }

        public Customer Customer { get; set; }
        public Product Product { get; set; }
    }
}
