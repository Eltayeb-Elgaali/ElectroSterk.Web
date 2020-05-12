using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

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
    }
}