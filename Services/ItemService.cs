using Enterprises.Entities;
using Enterprises.Repositories;

namespace Enterprises.Services
{
    public interface IItemService
    {
        Task<List<MstItem>> GetAllAsync();
        Task<MstItem?> GetAsync(int id);
        Task<MstItem> CreateAsync(MstItem item);
        Task UpdateAsync(MstItem item);
        Task DeleteAsync(int id);
    }

    public class ItemService : IItemService
    {
        private readonly ItemRepository _repo;
        public ItemService(ItemRepository repo) { _repo = repo; }
        public Task<List<MstItem>> GetAllAsync() => _repo.GetAllAsync();
        public Task<MstItem?> GetAsync(int id) => _repo.GetAsync(id);
        public Task<MstItem> CreateAsync(MstItem item) => _repo.CreateAsync(item);
        public Task UpdateAsync(MstItem item) => _repo.UpdateAsync(item);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
