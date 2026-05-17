using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enterprises.Entities;

namespace Enterprises.Configurations
{
    public class TrnOrderDetailConfiguration : IEntityTypeConfiguration<TrnOrderDetail>
    {
        public void Configure(EntityTypeBuilder<TrnOrderDetail> builder)
        {
            builder.HasKey(d => d.OrderDetailNo);
            builder.Property(d => d.Rate).HasPrecision(9,2);
            builder.Property(d => d.ItemAmount).HasPrecision(9,2);
            builder.Property(d => d.Discount).HasPrecision(9,2);
            builder.Property(d => d.TotalAmount).HasPrecision(9,2);

            builder.HasOne(d => d.OrderHead)
                   .WithMany(h => h.Details)
                   .HasForeignKey(d => d.OrderNo)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
