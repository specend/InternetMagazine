using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class Category
    {
        [Key]
        public int Id_category { get; set; }
        public string Name_category { get; set; }

        public List<Product> Products { get; set; }
    }
}
