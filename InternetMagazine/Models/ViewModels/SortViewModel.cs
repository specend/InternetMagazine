using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{

    public class SortViewModel
    {
        public string AttributeSort { get; private set; } // значение для сортировки по имени
        public string TypeSort { get; private set; }    // значение для сортировки по цене
        public string Current { get; private set; }     // текущее значение сортировки


        public SortViewModel(string typeSort, string attributeSort)
        {
            AttributeSort = attributeSort;
            TypeSort = typeSort;
            Current = AttributeSort + TypeSort;
        }
    }
}
