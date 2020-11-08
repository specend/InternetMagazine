using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class Order
    {
        [Key]
        public int Id_order { get; set; }

        public int Id_Record { get; set; }
        public int Id_product { get; set; }

        public int Count { get; set; }
        public int SummOrder { get; set; }

        

        public WriteOfOrder WriteOfOrder { get; set; }
        public Product Product { get; set; }
    }
}
