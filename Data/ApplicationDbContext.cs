using Microsoft.EntityFrameworkCore;
using Enterprises.Entities;

namespace Enterprises.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {
        }

        public DbSet<ExampleEntity> Examples { get; set; } = null!;
        public DbSet<Customer> Customers { get; set; } = null!;
        public DbSet<MstAccount> MstAccounts { get; set; } = null!;
        public DbSet<MstItem> MstItems { get; set; } = null!;
        public DbSet<TrnOrderHead> TrnOrderHeads { get; set; } = null!;
        public DbSet<TrnOrderDetail> TrnOrderDetails { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
