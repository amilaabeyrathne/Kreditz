using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KreditzBankData.Shared.Data.Configurations
{
    public class IngestionLogConfiguration : IEntityTypeConfiguration<IngestionLog>
    {
        public void Configure(EntityTypeBuilder<IngestionLog> builder)
        {
            builder.ToTable("ingestion_logs");

            builder.HasKey(i => i.Id);

            // BatchId is the public-facing identifier stamped on every record in the batch
            builder.Property(i => i.BatchId)
                .IsRequired();

            builder.Property(i => i.FileName)
                .IsRequired()
                .HasMaxLength(500);

            // SHA-256 produces a 64-character hex string
            builder.Property(i => i.FileHash)
                .IsRequired()
                .HasMaxLength(64);

            builder.Property(i => i.StartedAt)
                .IsRequired();

            builder.Property(i => i.CompletedAt);

            builder.Property(i => i.Status)
                .IsRequired()
                .HasMaxLength(50);

            builder.Property(i => i.RecordsProcessed)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(i => i.ErrorMessage)
                .HasMaxLength(2000);

            builder.Property(i => i.CreatedAt)
                .IsRequired();

            // BatchId must be globally unique — used as FK principal key by Balances and Transactions
            builder.HasIndex(i => i.BatchId)
                .IsUnique()
                .HasDatabaseName("ix_ingestion_logs_batch_id");

            // Idempotency check: has this file hash already been ingested?
            builder.HasIndex(i => i.FileHash)
                .HasDatabaseName("ix_ingestion_logs_file_hash");
        }
    }
}
