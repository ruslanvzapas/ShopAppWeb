using Humanizer.Localisation;
using Microsoft.EntityFrameworkCore;

namespace ShopAppWebUI.Repositories
{

    public interface ICollectionRepository
    {
        Task AddCollection(Collection collection);
        Task UpdateCollection(Collection collection);
        Task<Collection?> GetCollectionById(int id);
        Task DeleteCollection(Collection collection);
        Task<IEnumerable<Collection>> GetCollections();
    }

    public class CollectionRepository : ICollectionRepository
    {
        private readonly ApplicationDbContext _context;
        public CollectionRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task AddCollection(Collection collection)
        {
            _context.Collections.Add(collection);
            await _context.SaveChangesAsync();
        }
        public async Task UpdateCollection(Collection collection)
        {
            _context.Collections.Update(collection);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteCollection(Collection collection)
        {
            _context.Collections.Remove(collection);
            await _context.SaveChangesAsync();
        }

        public async Task<Collection?> GetCollectionById(int id)
        {
            return await _context.Collections.FindAsync(id);
        }

        public async Task<IEnumerable<Collection>> GetCollections()
        {
            return await _context.Collections.ToListAsync();
        }
    }
}
