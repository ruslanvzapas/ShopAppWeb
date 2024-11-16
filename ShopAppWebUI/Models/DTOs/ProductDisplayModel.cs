namespace ShopAppWebUI.Models.DTOs
{
    public class ProductDisplayModel
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Collection> Collections { get; set; }
        public string STerm { get; set; } = "";
        public int CollectionId { get; set; } = 0;

        public double? MinPrice { get; set; }
        public double? MaxPrice { get; set; }

    }
}
