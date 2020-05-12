using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Entities
{
    public class Sidebar
    {
        [Key]
        public int Id { get; set; }

        [AllowHtml]
        public string Body { get; set; }
    }
}
