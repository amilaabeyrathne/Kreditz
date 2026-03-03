using KreditzBankData.Shared.Data.Entities;

namespace KreditzBankData.Shared.IRepository
{
    public interface ICategoryRepository : IRepository<Categories>
    {
        Task<Categories?> GetByCategoryNameAsync(string categoryName, CancellationToken cancellationToken = default);
    }
}
