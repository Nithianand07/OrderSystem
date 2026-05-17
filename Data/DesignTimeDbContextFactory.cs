using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Enterprises.Data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
    {
        public ApplicationDbContext CreateDbContext(string[] args)
        {
            var builder = new DbContextOptionsBuilder<ApplicationDbContext>();
            // Use a placeholder connection string for design-time. Update if needed.
            builder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=EnterprisesDb;Trusted_Connection=True;");
            return new ApplicationDbContext(builder.Options);
        }
    }
}
