using Enterprises.Entities;
using Enterprises.Repositories;

namespace Enterprises.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _repo;

        public CustomerService(ICustomerRepository repo)
        {
            _repo = repo;
        }

        public Task<List<Customer>> GetAllAsync() => _repo.GetAllAsync();

        public Task<Customer?> GetAsync(int id) => _repo.GetAsync(id);

        public async Task<Customer> CreateAsync(Customer customer)
        {
            // Business rule: email must be unique
            var all = await _repo.GetAllAsync();
            if (all.Any(c => c.Email.Equals(customer.Email, StringComparison.OrdinalIgnoreCase)))
                throw new InvalidOperationException("Email must be unique");

            return await _repo.CreateAsync(customer);
        }

        public Task UpdateAsync(Customer customer) => _repo.UpdateAsync(customer);

        public Task DeleteAsync(int id) => _repo.DeleteAsync(id);

        public Task<bool> ExistsAsync(int id) => _repo.ExistsAsync(id);
    }
}
