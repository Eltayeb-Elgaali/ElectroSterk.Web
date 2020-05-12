using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.ModelBinding;
using DA;
using Entities;

namespace BLL
{
    public static class Pages
    {
        public static List<Page> AllPages()
        {
            return PageDa.GetPages();
        }

        public static bool AddPage(Page page)
        {
             return PageDa.AddPage(page);
        }

        public static Page Get(int id)
        {
            if (id > 0)      //resharper make it korter vertie
            {
                return PageDa.GetPage(id); //to not consume memory dont use var
            }

            return new Page();
        }

        public static void Update(Page page)
        {
            if (page != null)
            {
                PageDa.UpdatePage(page);
            }
           
        }

        public static void Delete(Page page)
        {
            if (!string.IsNullOrEmpty(page.Title))
            {
                PageDa.DeletePage(page);
            }
        }
    }
}
