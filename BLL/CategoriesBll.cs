using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using Entities;

namespace BLL
{
    public static class CategoriesBll
    {
        public static List<Category> Get()
        {
            return CategoryDa.GetCategories();
        }

        public static string Add(string name)
        {
            return CategoryDa.AddCategory(name);
        }
    }
}
