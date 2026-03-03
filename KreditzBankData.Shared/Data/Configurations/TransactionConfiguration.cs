using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KreditzBankData.Shared.Data.Configurations
{
    public class TransactionConfiguration : IEntityTypeConfiguration<Transaction>
    {
        public void Configure(EntityTypeBuilder<Transaction> builder)
        {
            builder.ToTable("transactions");

            builder.HasKey(t => t.Id);

            builder.Property(t => t.AccountId)
                .IsRequired();

            builder.Property(t => t.BatchId)
                .IsRequired();

            builder.Property(t => t.ExternalId)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(t => t.Amount)
                .IsRequired()
                .HasColumnType("numeric(18,2)");

            builder.Property(t => t.Currency)
                .IsRequired()
                .HasMaxLength(10);

            builder.Property(t => t.Description)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(t => t.TransactionDate)
                .IsRequired();

            builder.Property(t => t.BookingDate)
                .IsRequired();

            builder.Property(t => t.CategoryId)
                .IsRequired();

            builder.Property(t => t.IsAnomaly)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(t => t.AnomalyReasons)
                .HasMaxLength(1000);

            builder.Property(t => t.CreatedAt)
                .IsRequired();

            // Deduplication: same ExternalId must not appear twice for the same account
            builder.HasIndex(t => new { t.AccountId, t.ExternalId })
                .IsUnique()
                .HasDatabaseName("ix_transactions_account_id_external_id");

            // Reporting queries: filter by account + date range
            builder.HasIndex(t => new { t.AccountId, t.TransactionDate })
                .HasDatabaseName("ix_transactions_account_id_transaction_date");

            // Reporting queries: filter by category FK
            builder.HasIndex(t => t.CategoryId)
                .HasDatabaseName("ix_transactions_category_id");

            // Anomaly report queries
            builder.HasIndex(t => t.IsAnomaly)
                .HasDatabaseName("ix_transactions_is_anomaly");

            // Traceability: find all transactions from a specific ingestion batch
            builder.HasIndex(t => t.BatchId)
                .HasDatabaseName("ix_transactions_batch_id");

            // Relationship to IngestionLogs via BatchId
            builder.HasOne(t => t.IngestionLog)
                .WithMany(i => i.Transactions)
                .HasForeignKey(t => t.BatchId)
                .HasPrincipalKey(i => i.BatchId)
                .OnDelete(DeleteBehavior.Restrict);

            // Relationship to Categories
            builder.HasOne(t => t.Category)
                .WithMany(c => c.Transactions)
                .HasForeignKey(t => t.CategoryId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
