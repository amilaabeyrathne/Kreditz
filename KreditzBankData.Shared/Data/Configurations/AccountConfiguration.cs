using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KreditzBankData.Shared.Data.Configurations
{
    public class AccountConfiguration : IEntityTypeConfiguration<Account>
    {
        public void Configure(EntityTypeBuilder<Account> builder)
        {
            builder.ToTable("accounts");

            builder.HasKey(a => a.Id);

            builder.Property(a => a.AccountNumber)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.BankId)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(a => a.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(a => a.CreatedAt)
                .IsRequired();

            // Unique index — same account number should never be stored twice
            builder.HasIndex(a => a.AccountNumber)
                .IsUnique()
                .HasDatabaseName("ix_account_account_number");

            // Relationships
            builder.HasMany(a => a.Holders)
                .WithOne(h => h.Account)
                .HasForeignKey(h => h.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Balances)
                .WithOne(b => b.Account)
                .HasForeignKey(b => b.AccountId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(a => a.Transactions)
                .WithOne(t => t.Account)
                .HasForeignKey(t => t.AccountId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
