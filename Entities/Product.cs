using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Entities
{
    public class Product
    {
        public int Id { get; set; }  // IDENTITY
        [Required]
        public string Name { get; set; }
        [Required]
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
        [Required]
        public int CategoryId { get; set; }
        public string ImageName { get; set; }
        public virtual Category Category { get; set; }
        public virtual IEnumerable<SelectListItem> Categories { get; set; }
        public virtual IEnumerable<string> GalleryImages { get; set; }

        public virtual ICollection<Review> Reviews { get; set; }


        public Product()
        {
            Reviews = new List<Review>();
        }
    }
}
