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


        public static Category GetCategoryWithId(int id)
        {
            if (id > 0)      //resharper make it shorter
            {
                return CategoryDa.GetCategory(id); //to not consume memory dont use var
            }

            return new Category();
        }

        public static void Delete(Category category)
        {
            if (!string.IsNullOrEmpty(category.Name))
            {
                CategoryDa.DeleteCategory(category);
            }
        }

        public static string Update(string name, int id)
        {
            return CategoryDa.UpdateCategory(name, id);
        }
    }
}
