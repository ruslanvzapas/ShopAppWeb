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

        public async Task<IEnumerable<Product>> GetProducts(string sTerm = "", int collectionId = 0)
        {
            sTerm = sTerm.ToLower();
            IEnumerable<Product> products = await
                 (from product in _db.Products
                  join collection in _db.Collections
                   on product.CollectionId equals collection.Id
                  where string.IsNullOrWhiteSpace(sTerm) || (product != null && product.ProductName.ToLower().StartsWith(sTerm))
                  select new Product
                  {
                      Id = product.Id,
                      Image = product.Image,
                      ProductName = product.ProductName,
                      CollectionId = product.CollectionId,
                      Price = product.Price,
                      CollectionName = collection.CollectionName,
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
