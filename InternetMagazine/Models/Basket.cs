using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace InternetMagazine.Models
{
    public class Basket
    {
        
        [Key]
        public int Id_order { get; set; }
        //Внешние ключи
        public int Id_customer { get; set; }
        public int Id_product { get; set; }

        public int Count { get; set; }
        public int SummOrder { get; set; }

        
        //Свойства навигации
        public virtual Customer Customer { get; set; }
        public virtual Product Product { get; set; }
    }
}
