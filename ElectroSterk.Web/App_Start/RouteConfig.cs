using System.Web.Mvc;
using System.Web.Routing;

namespace ElectroSterk.Web
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");


            routes.MapRoute("Manage", "Manage/{action}/{id}", new { controller = "Manage", id = UrlParameter.Optional },
                new[] { "ElectroSterk.Web.Controllers" });

            routes.MapRoute("Account", "Account/{action}/{id}", new { controller = "Account", id = UrlParameter.Optional },
                new[] { "ElectroSterk.Web.Controllers" });

            routes.MapRoute("Cart", "Cart/{action}/{id}", new { controller = "Cart", action = "Index", id = UrlParameter.Optional },
                new[] { "ElectroSterk.Web.Controllers" });
            routes.MapRoute("Shop", "Shop/{action}/{name}", new { controller = "Shop", action = "Index", name = UrlParameter.Optional },
                new[] { "ElectroSterk.Web.Controllers" });
            routes.MapRoute("SidebarPartial", "Pages/SidebarPartial", new { controller = "Pages", action = "SidebarPartial" },
                new[] { "ElectroSterk.Web.Controllers" });
            routes.MapRoute("PagesMenuPartial", "Pages/PagesMenuPartial", new { controller = "Pages", action = "PagesMenuPartial" },
                new[] { "ElectroSterk.Web.Controllers" });
            routes.MapRoute("Pages", "{page}", new { controller = "Pages", action = "Index" },
                new[] { "ElectroSterk.Web.Controllers" });
            routes.MapRoute("Default", "", new {controller = "Pages", action = "Index"},
                new[] {"ElectroSterk.Web.Controllers"});

            //routes.MapRoute(
            //    name: "Default",
            //    url: "{controller}/{action}/{id}",
            //    defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            //);


            routes.MapRoute(
                name: "AdminArea",
                url: "{Area}/{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );

        }
    }
}
