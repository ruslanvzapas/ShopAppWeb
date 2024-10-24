using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShopAppWebUI.Models
{
    [Table("Collection")]
    public class Collection
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string CollectionName { get; set; }
        public List<Product> Products { get; set; }
    }
}
