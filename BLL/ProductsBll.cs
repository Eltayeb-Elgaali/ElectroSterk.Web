using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
    }
}
