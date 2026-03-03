using KreditzBankData.Shared.Data.Configurations;
using KreditzBankData.Shared.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace KreditzBankData.Shared.Data
{
    public class KreditzBankDataDbContext : DbContext
    {
        public KreditzBankDataDbContext(DbContextOptions<KreditzBankDataDbContext> options)
            : base(options)
        {
        }

        public DbSet<Account> Accounts => Set<Account>();
        public DbSet<AccountHolder> AccountHolders => Set<AccountHolder>();
        public DbSet<Balance> Balances => Set<Balance>();
        public DbSet<Transaction> Transactions => Set<Transaction>();
        public DbSet<IngestionLog> IngestionLogs => Set<IngestionLog>();
        public DbSet<Categories> Categories => Set<Categories>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new AccountHolderConfiguration());
            modelBuilder.ApplyConfiguration(new BalanceConfiguration());
            modelBuilder.ApplyConfiguration(new TransactionConfiguration());
            modelBuilder.ApplyConfiguration(new IngestionLogConfiguration());
            modelBuilder.ApplyConfiguration(new CategoryConfiguration());
        }
    }
}
