using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DA;
using Entities;

namespace BLL
{
    public static class Sidebars
    {
        public static Sidebar Get(int id)
        {
            return SidebarDa.GetSidebar(id);
        }

        public static void Update(Sidebar sidebar)
        {
            SidebarDa.UpdateSidbar(sidebar);
        }
    }
}
