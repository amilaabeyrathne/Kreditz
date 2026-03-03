using KreditzBankData.Shared.Data.Entities;

namespace KreditzBankData.Shared.IRepository
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account?> GetByAccountNumberAndBankIdAsync(string accountNumber, string bankId, CancellationToken cancellationToken = default);
    }
}
