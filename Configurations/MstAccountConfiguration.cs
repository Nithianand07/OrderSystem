using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Enterprises.Entities;

namespace Enterprises.Configurations
{
    public class MstAccountConfiguration : IEntityTypeConfiguration<MstAccount>
    {
        public void Configure(EntityTypeBuilder<MstAccount> builder)
        {
            builder.HasKey(a => a.AccountID);
            builder.Property(a => a.AccountType).HasMaxLength(8).IsRequired();
            builder.Property(a => a.AccountName).HasMaxLength(30).IsRequired();
            builder.Property(a => a.ContactNumber).HasMaxLength(12);
            builder.Property(a => a.EmailAddress).HasMaxLength(50);
        }
    }
}
