using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DA;
using Entities;

namespace ElectroSterk.Web.Controllers
{
    public class PagesController : Controller
    {
        // GET: Index
        public ActionResult Index(string page = "")
        {
            if (page == "")
                page = "home";
            Page modelPage ;

            using (var db = new ElectroSterkDbContext())
            {
                if (!db.Pages.Any(x => x.Title.Equals(page)))
                {
                    return RedirectToAction("Index", new { page = ""});
                }
            }

            using (var db = new ElectroSterkDbContext())
            {
                modelPage = db.Pages.FirstOrDefault(x => x.Title == page);
            }

            if (modelPage != null)
            {
                ViewBag.PageTitle = modelPage.Title;
            }

            if (modelPage.HasSidbar)
            {
                ViewBag.Sidebar = "Yes";
            }
            else
            {
                ViewBag.Sidebar = "No";
            }
            return View(modelPage);
        }

        public ActionResult PagesMenuPartial()
        {
            List<Page> pageList;
            using (var db = new ElectroSterkDbContext())
            {
                pageList = db.Pages.ToArray().OrderBy(x => x.Sorting).Where(x => x.Title != "home").ToList();
            }

            return PartialView(pageList);
        }

        public ActionResult SidebarPartial()
        {
            Sidebar sidebar;

            using (var db = new ElectroSterkDbContext())
            {
                sidebar = db.Sidebars.Find(1);
            }

            return PartialView(sidebar);
        }
    }
}