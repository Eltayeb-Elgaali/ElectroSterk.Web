using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class Category
    {
        [Key]
        public int Id { get; set; } // how to add auto incremented IDENTITY
        public string Name { get; set; }
        public int Sorting { get; set; }

    }
}
