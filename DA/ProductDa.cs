using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Entities;

namespace DA
{
    public static class ProductDa
    {
        public static Product GetProduct()
        {
            var product = new Product();
            using (var db = new ElectroSterkDbContext())
            {
                product.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
            }

            return product;
        }

        public static void AddProduct(Product product)
        {
            using (var db = new ElectroSterkDbContext())
            {
                if (product != null)
                {
                    db.Products.Add(product);
                    db.SaveChanges();
                }
                
            }



        //    if (string.IsNullOrEmpty(product.Name))
        //    {
        //        using (var db = new ElectroSterkDbContext())
        //        {
        //            product.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
        //            return product;
        //        }
        //    }


        //    using (var db = new ElectroSterkDbContext())
        //    {
        //        if (db.Products.Any(x=> x.Name == product.Name))
        //        {
        //            product.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                    
        //            return product;
        //        }
        //    }
        }

        public static List<Product> GetProducts(int? catId)
        {
            using (var db = new ElectroSterkDbContext())
            {
                return db.Products.ToArray()
                    .Where(x => catId == null || catId == 0 || x.CategoryId == catId)
                    .ToList();
            }
        }

        public static SelectList CategoriesViewBag()
        {
            using (var db = new ElectroSterkDbContext())
            {
                
                
                return new SelectList(db.Categories.ToList(),"Id", "Name");
               
            }
        }


        public static Product UpdateProduct(int id)
        {
            Product product;
            using (var db = new ElectroSterkDbContext())
            {
                product = db.Products.Find(id);
                if(product != null)
                {
                    product.Categories = new SelectList(db.Categories.ToList(), "Id", "Name");
                }
                
            }

            return product;
        }


        public static void UpdateProductExecute(Product model, int id)
        {
            Product product;
            using (var db = new ElectroSterkDbContext())
            {
                product = db.Products.Find(id);


                product.Name = model.Name;
                product.Description = model.Description;
                product.Price = model.Price;
                product.CategoryId = model.CategoryId;
                product.ImageName = model.ImageName;

                Category category = db.Categories.FirstOrDefault(x => x.Id == model.CategoryId);
                product.CategoryName = category.Name;
                db.SaveChanges();
            }
        }

        public static void DeleteProduct(int id)
        {
            using (var db = new ElectroSterkDbContext())
            {
                var product = db.Products.Find(id);
                if (product != null)
                {
                    db.Products.Remove(product);
                    db.SaveChanges();
                }
                
            }
        }

    }
}
