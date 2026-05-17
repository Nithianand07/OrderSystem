using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enterprises.Entities;

namespace Enterprises.Configurations
{
    public class MstItemConfiguration : IEntityTypeConfiguration<MstItem>
    {
        public void Configure(EntityTypeBuilder<MstItem> builder)
        {
            builder.HasKey(i => i.ItemID);
            builder.Property(i => i.ItemCode).HasMaxLength(20).IsRequired();
            builder.HasIndex(i => i.ItemCode).IsUnique();
            builder.Property(i => i.ItemName).HasMaxLength(50);
            builder.Property(i => i.ItemUOM).HasMaxLength(10);
            builder.Property(i => i.PurchasePrice).HasPrecision(9,2);
            builder.Property(i => i.SalePrice).HasPrecision(9,2);
        }
    }
}
