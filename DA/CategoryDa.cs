using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DA
{
    public static class CategoryDa
    {
        public static List<Category> GetCategories()
        {
            using (var db = new ElectroSterkDbContext())
            {
                return db.Categories.ToArray().OrderBy(x => x.Sorting).ToList();
            }
        }

        public static string AddCategory(string name)
        {
            string id;
            using (var db = new ElectroSterkDbContext())
            {
                if (db.Categories.Any(x => x.Name == name)) return "titletaken"; 
                 var category = new Category
                {
                    Name = name,
                    Sorting = 100
                };

                db.Categories.Add(category);
                db.SaveChanges();
                id = category.Id.ToString();
            }

            return id;
        }


        public static Category GetCategory(int id)
        {
            using (var db = new ElectroSterkDbContext())
            {
                return db.Categories.FirstOrDefault(x => x.Id == id);

            }
        }


        public static void DeleteCategory(Category category)
        {
            using (var db = new ElectroSterkDbContext())
            {
                db.Entry(category).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }

        public static string UpdateCategory(string name, int id)
        {
            using (var db = new ElectroSterkDbContext())
            {
                if (db.Categories.Any(x => x.Name == name)) return "titleused";

                var category = db.Categories.FirstOrDefault(x=>x.Id == id);

                category.Name = name;
                db.SaveChanges();
            }

            return "done";
        }
    }
}
