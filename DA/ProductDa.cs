using System;
using System.Collections.Generic;
using System.Linq;
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
    }
}
