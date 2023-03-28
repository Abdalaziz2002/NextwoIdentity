using System.ComponentModel.DataAnnotations.Schema;

namespace NextwoIdentity.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? price { get; set; }
        public string? type { get; set; }
        [ForeignKey("Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

    }
}
