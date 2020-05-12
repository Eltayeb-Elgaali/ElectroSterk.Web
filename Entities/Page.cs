using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Entities
{
    public class Page
    {
        [Key]
        public int Id { get; set; } // how to set auto incremented IDENTITY
        [Required]
        [StringLength(50,MinimumLength = 3)]
        public string Title { get; set; }
        [Required]
        [StringLength(int.MaxValue, MinimumLength = 3)]

        [AllowHtml]
        public string Body { get; set; }
        public int Sorting { get; set; }
        public bool HasSidbar { get; set; }
    }
}
