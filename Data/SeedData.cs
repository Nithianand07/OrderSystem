using Enterprises.Entities;

namespace Enterprises.Data
{
    public static class SeedData
    {
        public static void Initialize(ApplicationDbContext context)
        {
            if (context.Customers.Any()) return;

            context.Customers.AddRange(
                new Customer { Name = "Alice", Email = "alice@example.com", CreatedAt = DateTime.UtcNow },
                new Customer { Name = "Bob", Email = "bob@example.com", CreatedAt = DateTime.UtcNow },
                new Customer { Name = "Carol", Email = "carol@example.com", CreatedAt = DateTime.UtcNow }
            );

            context.SaveChanges();
        }
    }
}
