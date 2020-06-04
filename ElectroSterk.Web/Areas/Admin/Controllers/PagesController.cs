﻿using System.Linq;
using System.Web.Mvc;
using BLL;
using DA;
using Entities;

namespace ElectroSterk.Web.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PagesController : Controller
    {
        // GET: Admin/Pages
        public ActionResult Index()
        {

            return View(Pages.AllPages());
        }

        public ActionResult AadPage()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AadPage(Page model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            using (var db = new ElectroSterkDbContext())
            {
                var page = new Page();
                page.Title = model.Title;
                if (db.Pages.Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("", "This title is already exist") ;
                    return View(model);
                }


                page.Body = model.Title;
                page.HasSidbar = model.HasSidbar;
                page.Sorting = 100;
                Pages.AddPage(page);
            }

            TempData["SM"] = "You have added a new page";
            return RedirectToAction("AadPage");
        }

        
        public ActionResult EditPage(int id)
        {
            var page = Pages.Get(id);
            if (page == null)
            {
                return Content("This page is not exist");
            }

            return View(page);

        }

        [HttpPost]
        public ActionResult EditPage(Page model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            int id = model.Id;
            var page = Pages.Get(id);
            page.Title = model.Title;

            using (var db = new ElectroSterkDbContext())
            {
                if (db.Pages.Where(x => x.Id != id).Any(x => x.Title == model.Title))
                {
                    ModelState.AddModelError("","That Title is already exist !");
                    return View(model);
                }

                page.Body = model.Body;
                page.HasSidbar = model.HasSidbar;
            }

            Pages.Update(page);
            TempData["SM"] = "You have edited the page!";
            return RedirectToAction("EditPage");

        }

        public ActionResult PageDetails(int id)
        {
            var page = Pages.Get(id);
            if (page == null)
            {
                return Content("The Page does not exist");
            }

            return View(page);
        }

        public ActionResult DeletePage(int id)
        {
            Pages.Delete(Pages.Get(id));
            return RedirectToAction("Index");
        }

        [HttpPost]
        public void ReorderPages(int[] id)
        {
            using (var db = new ElectroSterkDbContext())
            {
                
                int count = 1;

                Page page;
                
                foreach (var pageId in id)
                {
                    page = db.Pages.Find(pageId);
                    page.Sorting = count; 
                    db.SaveChanges();
                    count++;
                }
            }
        }

        public ActionResult EditSidebar()
        {
            return View(Sidebars.Get(1));
        }

        [HttpPost]
        public ActionResult EditSidebar(Sidebar model)
        {
            var sidebar = Sidebars.Get(1);
            sidebar.Body = model.Body;
            Sidebars.Update(sidebar);

            TempData["SM"] = "You have edited the sidebar";
            return RedirectToAction("EditSidebar");

        }
    }
}