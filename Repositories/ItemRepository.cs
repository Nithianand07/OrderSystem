using Enterprises.Data;
using Enterprises.Entities;

namespace Enterprises.Repositories
{
    public class ItemRepository : RepositoryBase<MstItem>
    {
        public ItemRepository(ApplicationDbContext db) : base(db) { }
    }
}
