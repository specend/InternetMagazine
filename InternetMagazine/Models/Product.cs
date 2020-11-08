using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class Product
    {
        
        [Key]
        public int Id_Product { get; set; }

        public string Name { get; set; }
        public int Price { get; set; }
        public int Count { get; set; }
        public string Description { get; set; }
        public string NameImage { get; set; }
        public byte[] Image { get; set; }
        public int Id_category { get; set; }//FK


        public Category Category { get; set; } //Навигационное свойство

        public List<Basket> Baskets { get; set; }
        public List<Order> Orders { get; set; }
        public List<Review> Reviews { get; set; }
    }
}
