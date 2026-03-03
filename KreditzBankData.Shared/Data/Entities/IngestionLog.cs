namespace KreditzBankData.Shared.Data.Entities
{
    public class IngestionLog : BaseEntity
    {
        public Guid BatchId { get; set; }

        public string FileName { get; set; } = string.Empty;

        /// <summary>SHA-256 hash of the file — used for idempotency (prevents re-ingesting same file)</summary>
        public string FileHash { get; set; } = string.Empty;

        public DateTimeOffset StartedAt { get; set; }
        public DateTimeOffset? CompletedAt { get; set; }

        /// <summary>Pending | Success | Failed</summary>
        public string Status { get; set; } = string.Empty;

        public int RecordsProcessed { get; set; }
        public string? ErrorMessage { get; set; }

        // Navigation properties
        public ICollection<Balance> Balances { get; set; } = new List<Balance>();
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
