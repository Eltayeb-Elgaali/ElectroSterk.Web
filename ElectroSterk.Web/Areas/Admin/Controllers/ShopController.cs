using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;
using DA;
using Entities;

namespace ElectroSterk.Web.Areas.Admin.Controllers
{
    public class ShopController : Controller
    {
        // GET: Admin/Shop
        public ActionResult Categories()
        {
            
            return View(CategoriesBll.Get());
        }

        [HttpPost]
        public string AddNewCategory(string catName)
        {
            
           var id = CategoriesBll.Add(catName);
           return id;
        }

        [HttpPost]
        public void ReorderCategories(int[] id)
        {
            using (var db = new ElectroSterkDbContext())
            {

                int count = 1;

                Category category;

                foreach (var CategoryId in id)
                {
                    category = db.Categories.Find(CategoryId);
                    category.Sorting = count;
                    db.SaveChanges();
                    count++;
                }
            }
        }


        public ActionResult DeleteCategory(int id)
        {
            CategoriesBll.Delete(CategoriesBll.GetCategoryWithId(id));
            return RedirectToAction("Categories");
        }

        public string EditCategory(string newCategoryName, int id)
        {
            return CategoriesBll.Update(newCategoryName, id);
        }
    }
}