using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DA;
using Entities;

namespace BLL
{
    public static class ProductsBll
    {
        public static Product Get()
        {
            return ProductDa.GetProduct();
        }

        public static void Add(Product product)
        {
            ProductDa.AddProduct(product);
        }

        public static List<Product> ListProducts(int? catId)
        {
            return ProductDa.GetProducts(catId);
        }

        public static SelectList CategoriesVb() 
        {
            return ProductDa.CategoriesViewBag();
        }
    }
}
