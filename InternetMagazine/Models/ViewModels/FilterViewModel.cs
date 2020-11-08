using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace InternetMagazine.Models
{
    public class FilterViewModel
    {
        public SelectList Categories { get; private set; }
        public int? SelectedCategory { get; private set; }
        public string SelectedName { get; private set; }

        public FilterViewModel(List<Category> categories, int? category, string name)
        {
            categories.Insert(0, new Category() { Id_category = 0, Name_category = "Всё" });
            Categories = new SelectList(categories, "Id_category", "Name_category", category);
            SelectedCategory = category;
            SelectedName = name;
        }
    }
}
