namespace KreditzBankData.Shared.Data.Entities
{
    public class Account : BaseEntity
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;

        public ICollection<AccountHolder> Holders { get; set; } = new List<AccountHolder>();
        public ICollection<Balance> Balances { get; set; } = new List<Balance>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
