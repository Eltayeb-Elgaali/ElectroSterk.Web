using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using BLL;
using DA;
using ElectroSterk.Web.Areas.Admin.Data.Shop;
using Entities;
using PagedList;

namespace ElectroSterk.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ShopController : Controller
    {
        // GET: Admin/Shop/Categories
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

        public ActionResult Products(int? page, int? catId)
        {
            var products = ProductsBll.ListProducts(catId);
            var pageNumber = page ?? 1;
            

            ViewBag.Categories = ProductsBll.CategoriesVb();

            ViewBag.SelectedCat = catId.ToString();

             
            var onePageOfProducts = products.ToPagedList(pageNumber, 3);
            ViewBag.OnePageOfProducts = onePageOfProducts;
            return View(products);
        }

        public ActionResult EditProduct(int id)
        {
            var product = ProductsBll.Update(id);
            if (product == null)
            {
                return Content("The product does not exist");
            }

            product.GalleryImages =
                Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(x => Path.GetFileName(x));
            return View(product);
        }


        [HttpPost]
        public ActionResult EditProduct(Product product, HttpPostedFileBase ProductPic)
        {

            int id = product.Id;
            using (var db = new ElectroSterkDbContext())
            {
                product.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }
            product.GalleryImages =
                Directory.EnumerateFiles(Server.MapPath("~/Images/Uploads/Products/" + id + "/Gallery/Thumbs"))
                    .Select(x => Path.GetFileName(x));
            if (!ModelState.IsValid)
            {
                return View(product);
            }

            using (var db = new ElectroSterkDbContext())
            {
                if (db.Products.Where(x => x.Id != id).Any(x => x.Name == product.Name))
                {
                    ModelState.AddModelError("", "That product name is taken");
                    return View(product);
                }
            }


            ProductsBll.UpdateProductExecute(product,id);

            TempData["SM"] = "You have edited the product!";

            // update image and delete files
            if (ProductPic != null && ProductPic.ContentLength > 0)
            {
                string ext = ProductPic.ContentType.ToLower();

                if (ext != "image/jpg" &&
                    ext != "image/jpeg" &&
                    ext != "image/gif" &&
                    ext != "image/jpeg" &&
                    ext != "image/x-png" &&
                    ext != "image/png")
                {
                    using (var db = new ElectroSterkDbContext())
                    {
                        ModelState.AddModelError("", "Invalid Image!");
                        return View(product);

                    }
                }

                var mainDir = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));

                
                var tempPath1 = Path.Combine(mainDir.ToString(), "Products\\" + id.ToString());
                var tempPath2  = Path.Combine(mainDir.ToString(), "Products\\" + id.ToString() + "\\Thumbs");

                DirectoryInfo directory1 = new DirectoryInfo(tempPath1);
                DirectoryInfo directory2 = new DirectoryInfo(tempPath2);

                foreach (FileInfo file2 in directory1.GetFiles())
                {
                    file2.Delete();
                }

                foreach (FileInfo file3 in directory2.GetFiles())
                {
                    file3.Delete();
                }

                string imageName = ProductPic.FileName;
                using (var db = new ElectroSterkDbContext())
                {
                    Product prod = db.Products.Find(id);
                    if (prod != null)
                    {
                        prod.ImageName = imageName;
                        db.SaveChanges();
                    }
                     
                }

                var path = string.Format("{0}\\{1}", tempPath1, imageName);
                var path1 = string.Format("{0}\\{1}", tempPath2, imageName);

                ProductPic.SaveAs(path);

                WebImage img = new WebImage(ProductPic.InputStream);
                img.Resize(200, 200);
                img.Save(path1);
            }

            return RedirectToAction("EditProduct");
        }

        public ActionResult DeleteProduct(int id)       
        {
            ProductsBll.Delete(id);
            var baseDirectory = new DirectoryInfo(string.Format("{0}Images\\Uplaoads",Server.MapPath(@"\")));
            string tempPath = Path.Combine(baseDirectory.ToString(), "Products\\" + id.ToString());

            if(Directory.Exists((tempPath)))
                Directory.Delete(tempPath, true);

            return RedirectToAction("Products");
        }

        [HttpPost]
        public void SaveGalleryImages(int id)
        {
            foreach (string fileName in Request.Files)
            {
                HttpPostedFileBase file = Request.Files[fileName];

                if (file != null && file.ContentLength > 0)
                {
                    var baseDirectory = new DirectoryInfo(string.Format("{0}Images\\Uploads", Server.MapPath(@"\")));
                    string tempPath1 = Path.Combine(baseDirectory.ToString(), 
                        "Products\\" + id.ToString() + "\\Gallery");
                    string tempPath2 = Path.Combine(baseDirectory.ToString(),
                        "Products\\" + id.ToString() + "\\Gallery\\Thumbs");

                    var path = string.Format("{0}\\{1}", tempPath1, file.FileName);
                    var path2 = string.Format("{0}\\{1}", tempPath2, file.FileName);

                    file.SaveAs(path);
                    WebImage img = new WebImage(file.InputStream);
                    img.Resize(200, 200);
                    img.Save(path2);

                }
            }
        }

        public void DeleteImage(int id, string imageName)
        {
            string path1 = Request.MapPath("~/Images/Uploads/Products" + id.ToString() + "/Gallery/" + imageName);
            string path2 = Request.MapPath("~/Images/Uploads/Products" + id.ToString() + "/Gallery/Thumbs/" + imageName);

            if(System.IO.File.Exists(path1))
                System.IO.File.Delete(path1);

            if (System.IO.File.Exists(path2))
                System.IO.File.Delete(path2);
        }

        public ActionResult Orders()
        {
            List<OrdersForAdmin> ordersForAdmin = new List<OrdersForAdmin>();

            using (var db = new ElectroSterkDbContext())
            {
                List<Order> orders = db.Orders.ToList();

                foreach (var order in orders)
                {
                    Dictionary<string, int> productsAndQuantity = new Dictionary<string, int>();

                    decimal total = 0m;

                    List<OrderDetails> orderDetailsList =
                        db.OrderDetails.Where(x => x.OrderId == order.OrderId).ToList();

                    OrderUser user = db.OrderUsers.Where(x => x.Id == order.UserId).FirstOrDefault();
                    string username = user.UserName;

                    foreach (var orderDetails in orderDetailsList)
                    {
                        Product product = db.Products.Where(x => x.Id == orderDetails.ProductId).FirstOrDefault();

                        decimal price = product.Price;
                        string productName = product.Name;
                        productsAndQuantity.Add(productName, orderDetails.Quantity);
                        total += orderDetails.Quantity * price;
                    }

                    ordersForAdmin.Add(new OrdersForAdmin()
                    {
                        OrderNumber = order.OrderId,
                        UserName = username,
                        Total = total,
                        ProductsAndQuantity = productsAndQuantity,
                        CreatedOn = order.CreatedOn
                    });
                }
            }
            return View(ordersForAdmin);
        }

        [HttpPost]
        public ActionResult SendReview(Review review, double rating)
        {


            using (var db = new ElectroSterkDbContext())
            {
                string username = Session["username"].ToString();
                review.PostDate = DateTime.Now;
                review.OrderUserId = db.OrderUsers.Single(a => a.UserName.Equals(username)).Id;
                review.Rating = rating;
                db.Reviews.Add(review);
                db.SaveChanges();

            }
            return RedirectToAction("product-details", "Shop", new { id = review.ProductId });
        }
    }
}