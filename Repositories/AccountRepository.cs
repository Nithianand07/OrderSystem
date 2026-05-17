using Enterprises.Data;
using Enterprises.Entities;

namespace Enterprises.Repositories
{
    public class AccountRepository : RepositoryBase<MstAccount>
    {
        public AccountRepository(ApplicationDbContext db) : base(db) { }
    }
}
