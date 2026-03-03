using KreditzBankData.Shared.Data.Entities;

namespace KreditzBankData.Shared.IRepository
{
    public interface IIngestionLogRepository : IRepository<IngestionLog>
    {
        Task<IngestionLog?> GetByFileHashAsync(string fileHash, CancellationToken cancellationToken = default);
    }
}
