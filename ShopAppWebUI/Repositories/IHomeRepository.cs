using Humanizer.Localisation;

namespace ShopAppWebUI
{
    public interface IHomeRepository
    {
        Task<IEnumerable<Product>> GetProducts(string sTerm = "", int collectionId = 0);
        Task<IEnumerable<Collection>> Collections();
    }
}