using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entities;

namespace DA
{
    public class ElectroSterkDbContext:DbContext
    {
        public ElectroSterkDbContext():base(@"Data Source=.\sqlexpress;Initial Catalog=ElectroSterkDataBank;Integrated Security=True")
        {
            
        }

        public DbSet<Page> Pages { get; set; }
        public DbSet<Sidebar> Sidebars { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Product> Products { get; set; }
    }
}


// Enable-Migrations -projectName DA -StartUpProjectName ElectroSterk.Web
// add-migration InitialCreate -projectName DA -StartUpProjectName ElectroSterk.Web
// update-database -projectName DA -StartUpProjectName ElectroSterk.Web