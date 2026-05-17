using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enterprises.Entities;

namespace Enterprises.Configurations
{
    public class TrnOrderHeadConfiguration : IEntityTypeConfiguration<TrnOrderHead>
    {
        public void Configure(EntityTypeBuilder<TrnOrderHead> builder)
        {
            builder.HasKey(h => h.OrderNo);
            builder.Property(h => h.OrderType).HasMaxLength(8).IsRequired();
            builder.Property(h => h.ItemAmount).HasPrecision(9,2);
            builder.Property(h => h.Discount).HasPrecision(9,2);
            builder.Property(h => h.TotalAmount).HasPrecision(9,2);
        }
    }
}
