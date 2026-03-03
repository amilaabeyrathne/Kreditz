using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KreditzBankData.Shared.Data
{
    /// <summary>
    /// Used exclusively by dotnet-ef design-time tools (migrations).
    /// Not referenced at runtime — the real DbContext is registered via DI in each service.
    /// </summary>
    public class KreditzBankDataDbContextFactory : IDesignTimeDbContextFactory<KreditzBankDataDbContext>
    {
        public KreditzBankDataDbContext CreateDbContext(string[] args)
        {
            var options = new DbContextOptionsBuilder<KreditzBankDataDbContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=kreditz_bankdata;Username=kreditz;Password=kreditz_secret")
                .Options;

            return new KreditzBankDataDbContext(options);
        }
    }
}
