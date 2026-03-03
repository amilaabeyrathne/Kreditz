using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KreditzBankData.Shared.Data.Configurations
{
    public class AccountHolderConfiguration : IEntityTypeConfiguration<AccountHolder>
    {
        public void Configure(EntityTypeBuilder<AccountHolder> builder)
        {
            builder.ToTable("account_holders");

            builder.HasKey(h => h.Id);

            builder.Property(h => h.AccountId)
                .IsRequired();

            builder.Property(h => h.Name)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(h => h.CreatedAt)
                .IsRequired();

            builder.HasIndex(h => h.AccountId)
                .HasDatabaseName("ix_account_holders_account_id");
        }
    }
}
