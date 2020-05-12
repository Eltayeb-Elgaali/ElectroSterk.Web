using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DA
{
    public static class SidebarDa
    {
        public static Sidebar GetSidebar(int id)
        {
            using (var db = new ElectroSterkDbContext())
            {
                return db.Sidebars.FirstOrDefault(x=>x.Id == id);
            }
        }

        public static void UpdateSidbar(Sidebar sidebar)
        {
            using (var db = new ElectroSterkDbContext())
            {
                db.Entry(sidebar).State = EntityState.Modified;
                db.SaveChanges() ;
            }
        }
    }
}
