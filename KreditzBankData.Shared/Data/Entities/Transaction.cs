namespace KreditzBankData.Shared.Data.Entities
{
    public class Transaction : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Guid BatchId { get; set; }

        /// <summary>Source system transaction ID — used for deduplication across ingestion runs</summary>
        public string ExternalId { get; set; } = string.Empty;

        public decimal Amount { get; set; }
        public string Currency { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateOnly TransactionDate { get; set; }
        public DateOnly BookingDate { get; set; }

        /// <summary>Computed by CategorizationService: Income | InternalTransfer | Expense</summary>
        public Guid CategoryId { get; set; }

        /// <summary>Set by AnomalyDetectionService</summary>
        public bool IsAnomaly { get; set; }

        /// <summary>Semicolon-separated list of anomaly reasons, e.g. "Exceeds threshold; Duplicate"</summary>
        public string? AnomalyReasons { get; set; }

        // Navigation properties
        public Account Account { get; set; } = null!;
        public IngestionLog IngestionLog { get; set; } = null!;
        public Categories Category { get; set; } = null!;
    }
}
