namespace KreditzBankData.IngestionService.Models
{
    /// <summary>Root model for an incoming bank data JSON file.</summary>
    public class BankDataFile
    {
        public AccountData Account { get; set; } = null!;
        public List<BalanceData> Balances { get; set; } = new();
        public List<TransactionData> Transactions { get; set; } = new();
    }

    public class AccountData
    {
        public string AccountNumber { get; set; } = string.Empty;
        public string BankId { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;
        public List<AccountHolderData> Holders { get; set; } = new();
    }

    public class AccountHolderData
    {
        public string Name { get; set; } = string.Empty;

        /// <summary>Optional PII — will be stored encrypted.</summary>
        public string? IdentityNumber { get; set; }
    }

    public class BalanceData
    {
        public decimal Amount { get; set; }
        public string BalanceType { get; set; } = string.Empty;
    }

    public class TransactionData
    {
        /// <summary>Source-system transaction ID — used for deduplication.</summary>
        public string ExternalId { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        /// <summary>ISO 8601 date string, e.g. "2024-03-15".</summary>
        public string TransactionDate { get; set; } = string.Empty;

        /// <summary>ISO 8601 date string, e.g. "2024-03-15".</summary>
        public string BookingDate { get; set; } = string.Empty;
    }
}
