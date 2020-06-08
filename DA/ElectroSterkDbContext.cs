using System.Data.Entity;
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

        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetails> OrderDetails { get; set; }
        public DbSet<OrderUser> OrderUsers { get; set; }


    }
}


// Enable-Migrations -projectName DA -StartUpProjectName ElectroSterk.Web
// add-migration InitialCreate -projectName DA -StartUpProjectName ElectroSterk.Web
// update-database -projectName DA -StartUpProjectName ElectroSterk.Web