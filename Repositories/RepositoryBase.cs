using Microsoft.EntityFrameworkCore;
using Enterprises.Data;

namespace Enterprises.Repositories
{
    public class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected readonly ApplicationDbContext _db;

        public RepositoryBase(ApplicationDbContext db)
        {
            _db = db;
        }

        public virtual async Task<List<T>> GetAllAsync()
        {
            return await _db.Set<T>()

                .Where(x =>

                    EF.Property<bool>(x, "IsDelete") == false

                )

                .ToListAsync();
        }

        public virtual async Task<T?> GetAsync(int id)
        {
            return await _db.FindAsync<T>(id);
        }

        public virtual async Task<T> CreateAsync(T entity)
        {
            _db.Set<T>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        public virtual async Task UpdateAsync(T entity)
        {
            _db.Entry(entity).State = EntityState.Modified;
            await _db.SaveChangesAsync();
        }

        public virtual async Task DeleteAsync(int id)
        {
            var entity = await GetAsync(id);

            if (entity != null)
            {
                typeof(T)
                    .GetProperty("IsDelete")
                    ?.SetValue(entity, true);

                typeof(T)
                    .GetProperty("ActiveFlg")
                    ?.SetValue(entity, false);

                _db.Set<T>().Update(entity);

                await _db.SaveChangesAsync();
            }
        }

        public virtual async Task<bool> ExistsAsync(int id)
        {
            var entity = await GetAsync(id);
            return entity != null;
        }
    }
}
