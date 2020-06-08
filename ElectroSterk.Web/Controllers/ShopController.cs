using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA;
using Entities;

namespace ElectroSterk.Web.Controllers
{
    
    public class ShopController : Controller
    {
        // GET: Shop
        
        public ActionResult Index()
        {
            return RedirectToAction("Index", "Pages");
        }

        public ActionResult CategoryMenuPartial()
        {
            List<Category> categoryLlist;
            using (var db = new ElectroSterkDbContext())
            {
                categoryLlist = db.Categories.ToArray().OrderBy(x => x.Sorting).ToList();
            }

            return PartialView(categoryLlist);
        }

        public ActionResult Category(string name)
        {
            List<Product> productList;

            using (var db = new ElectroSterkDbContext())
            {
                
                Category category = db.Categories.Where(x => x.Name == name).FirstOrDefault();
                int catId = category.Id;

                productList = db.Products.ToArray().Where(x => x.CategoryId == catId).ToList();

                var productCategory = db.Products.FirstOrDefault(x => x.CategoryId == catId);
                ViewBag.CategoryName = productCategory.CategoryName;
            }

            return View(productList);
        }

        [ActionName("product-details")]
        public ActionResult ProductDetails(string name)
        {
            Product product;
            int id = 0;

            using (var db = new ElectroSterkDbContext())
            {
                if (!db.Products.Any(x => x.Name.Equals(name)))
                {
                    return RedirectToAction("index", "Shop");
                }

                product = db.Products.Where(x => x.Name == name).FirstOrDefault();

                id = product.Id;
            }

            product.GalleryImages =
                Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(x => Path.GetFileName(x));

            return View("ProductDetails", product);
        }
    }
}