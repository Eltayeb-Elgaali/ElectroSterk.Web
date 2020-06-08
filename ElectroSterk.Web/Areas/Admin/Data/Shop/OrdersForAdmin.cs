using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ElectroSterk.Web.Areas.Admin.Data.Shop
{
    public class OrdersForAdmin
    {
        public int OrderNumber { get; set; }
        public string UserName { get; set; }
        public decimal Total { get; set; }
        public Dictionary<string, int> ProductsAndQuantity { get; set; }
        public DateTime CreatedOn { get; set; } 

    }
}