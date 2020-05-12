using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(ElectroSterk.Web.Startup))]
namespace ElectroSterk.Web
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
