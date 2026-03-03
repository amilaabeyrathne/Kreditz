using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KreditzBankData.Shared.Data.Configurations
{
    public class BalanceConfiguration : IEntityTypeConfiguration<Balance>
    {
        public void Configure(EntityTypeBuilder<Balance> builder)
        {
            builder.ToTable("balances");

            builder.HasKey(b => b.Id);

            builder.Property(b => b.AccountId)
                .IsRequired();

            builder.Property(b => b.BatchId)
                .IsRequired();

            builder.Property(b => b.Amount)
                .IsRequired()
                .HasColumnType("numeric(18,2)");

            builder.Property(b => b.CreatedAt)
                .IsRequired();

            // Query pattern: fetch latest balance for an account
            builder.HasIndex(b => new { b.AccountId, b.CreatedAt })
                .HasDatabaseName("ix_balances_account_id_created_at");

            // Traceability: find all balances from a specific ingestion batch
            builder.HasIndex(b => b.BatchId)
                .HasDatabaseName("ix_balances_batch_id");

            // Relationship to IngestionLogs via BatchId
            builder.HasOne(b => b.IngestionLog)
                .WithMany(i => i.Balances)
                .HasForeignKey(b => b.BatchId)
                .HasPrincipalKey(i => i.BatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
