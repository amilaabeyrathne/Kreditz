using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace KreditzBankData.Shared.Data.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Categories>
    {
        public void Configure(EntityTypeBuilder<Categories> builder)
        {
            builder.ToTable("categories");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Category)
                .IsRequired()
                .HasMaxLength(100);

            builder.Property(c => c.IsActive)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(c => c.CreatedAt)
                .IsRequired();

            // Cache warm-up query filters on IsActive — index speeds this up at 10,000 rules
            builder.HasIndex(c => c.IsActive)
                .HasDatabaseName("ix_categories_is_active");

        }
    }
}
