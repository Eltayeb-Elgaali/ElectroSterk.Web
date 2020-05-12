using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DA
{
    public static class PageDa
    {
        public static List<Page> GetPages()
        {
            using (var db = new ElectroSterkDbContext())
            {
                return db.Pages.ToArray().OrderBy(x => x.Sorting).ToList();
            }
        }

        public static bool AddPage(Page page)
        {

            using (var db = new ElectroSterkDbContext())
            {
                db.Pages.Add(page);
                return db.SaveChanges() > 0;

            }

        }

        public static Page GetPage(int id)
        {
            using (var db = new ElectroSterkDbContext())
            {
                return db.Pages.FirstOrDefault(x => x.Id == id);

            }
        }

        public static bool UpdatePage(Page page)
        {
            using (var db = new ElectroSterkDbContext())
            {
                db.Entry(page).State = EntityState.Modified;
                return db.SaveChanges() > 0;
            }
        }

        public static void DeletePage(Page page)
        {
            using (var db = new ElectroSterkDbContext())
            {
                db.Entry(page).State = EntityState.Deleted;
                db.SaveChanges();
            }
        }
    }
}
