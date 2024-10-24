using System.ComponentModel.DataAnnotations;

namespace ShopAppWebUI.Models.DTOs
{
    public class CollectionDTO
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(40)]
        public string CollectionName { get; set; }
    }
}
