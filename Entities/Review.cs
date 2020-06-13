using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities
{
    public class Review
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public double? Rating { get; set; }
        public DateTime? PostDate { get; set; }
        public int? ProductId { get; set; }
        public int? OrderUserId { get; set; }


        public OrderUser OrderUser { get; set; }
        public Product Product { get; set; }
    }
}
