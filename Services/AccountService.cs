using Enterprises.Entities;
using Enterprises.Repositories;

namespace Enterprises.Services
{
    public interface IAccountService
    {
        Task<List<MstAccount>> GetAllAsync();
        Task<MstAccount?> GetAsync(int id);
        Task<MstAccount> CreateAsync(MstAccount account);
        Task UpdateAsync(MstAccount account);
        Task DeleteAsync(int id);
        
    }

    public class AccountService : IAccountService
    {
        private readonly AccountRepository _repo;
        public AccountService(AccountRepository repo) { _repo = repo; }
        public Task<List<MstAccount>> GetAllAsync() => _repo.GetAllAsync();
        public Task<MstAccount?> GetAsync(int id) => _repo.GetAsync(id);
        public Task<MstAccount> CreateAsync(MstAccount account) => _repo.CreateAsync(account);

        public Task UpdateAsync(MstAccount account) => _repo.UpdateAsync(account);
        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);
    }
}
