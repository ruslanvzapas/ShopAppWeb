using Microsoft.EntityFrameworkCore;

namespace ShopAppWebUI.Repositories
{
    public class HomeRepository : IHomeRepository
    {
        private readonly ApplicationDbContext _db;

        public HomeRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Collection>> Collections()
        {
            return await _db.Collections.ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int collectionId = 0, double? minPrice = null, double? maxPrice = null)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await
                 (from product in _db.Products
                  join collection in _db.Collections
                  on product.CollectionId equals collection.Id
                  join stock in _db.Stocks
                  on product.Id equals stock.ProductId
                  into product_stocks
                  from productWithStock in product_stocks.DefaultIfEmpty()
                  where string.IsNullOrWhiteSpace(sTerm) || (product != null && 

                  product.ProductName.ToLower().StartsWith(sTerm)) &&
                  (!minPrice.HasValue || product.Price >= minPrice) && 
                  (!maxPrice.HasValue || product.Price <= maxPrice)
                  select new Product
                  {
                      Id = product.Id,
                      Image = product.Image,
                      ProductName = product.ProductName,
                      CollectionId = product.CollectionId,
                      Price = product.Price,
                      CollectionName = collection.CollectionName,
                      Quantity = productWithStock == null ? 0 : productWithStock.Quantity,
                  }
                ).ToListAsync();

            if (collectionId > 0)
            {

                products = products.Where(a => a.CollectionId == collectionId).ToList();
            }
            return products;

        }

    }
}
