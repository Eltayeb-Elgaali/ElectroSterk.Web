using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Helpers;
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

        public ActionResult AddProduct()
        {
            return View(ProductsBll.Get());
        }

        [HttpPost]
        public ActionResult AddProduct(Product model, HttpPostedFileBase ProductPic)
        {
            int id;
            if (!ModelState.IsValid)
            {
                using (var db = new ElectroSterkDbContext())
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    return View(model);
                }
            }

            using (var db = new ElectroSterkDbContext())
            {
                if (db.Products.Any(x => x.Name == model.Name))
                {
                    model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    ModelState.AddModelError("","This product is already exist");
                    return View(model);
                }
            }

            using (var db = new ElectroSterkDbContext())
            {
                var category = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);

                var newProduct = new Product()
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CategoryId = model.CategoryId,
                    CategoryName = category.Name
                };
                ProductsBll.Add(newProduct);
                
                id = newProduct.Id;
                

            }

            TempData["Msg"] = "The product has been aded";

            var mainDir = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

            var tempPath1 = Path.Combine(mainDir.ToString(), "Products");
            var tempPath2 = Path.Combine(mainDir.ToString(), "Products\\" + id.ToString());
            var tempPath3 = Path.Combine(mainDir.ToString(), "Products\\" + id.ToString() + "\\Thumbs" );
            var tempPath4 = Path.Combine(mainDir.ToString(), "Products\\" + id.ToString() + "\\Gallery" );
            var tempPath5 = Path.Combine(mainDir.ToString(), "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

            if (!Directory.Exists(tempPath1)) Directory.CreateDirectory(tempPath1);
            if (!Directory.Exists(tempPath2)) Directory.CreateDirectory(tempPath2);
            if (!Directory.Exists(tempPath3)) Directory.CreateDirectory(tempPath3);
            if (!Directory.Exists(tempPath4)) Directory.CreateDirectory(tempPath4);
            if (!Directory.Exists(tempPath5)) Directory.CreateDirectory(tempPath5);

            if (ProductPic != null && ProductPic.ContentLength > 0)
            {
                var ext = ProductPic.ContentType.ToLower();
                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/gif" &&
                    ext != "image/pjpeg" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (var db = new ElectroSterkDbContext())
                    {
                        
                            model.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                            ModelState.AddModelError("", "Invalid Image!");
                            return View(model);
                        
                    }
                }
           

            
                var image = ProductPic.FileName;
                using (var db = new ElectroSterkDbContext())
                {
                    var productForChangeName = db.Products.FirstOrDefault(x => x.Id == id);
                    productForChangeName.ImageName = image;
                    db.SaveChanges();
                }

                var path = string.Format("{0}\\{1}", tempPath2, image);
                var path1 = string.Format("{0}\\{1}", tempPath3, image);

                ProductPic.SaveAs(path);

                WebImage img = new WebImage(ProductPic.InputStream);
                img.Resize(200, 200);
                img.Save(path1);
            }

            return RedirectToAction("AddProduct");
        }
    }
}