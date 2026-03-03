namespace KreditzBankData.Shared.Data.Entities
{
    public class Balance : BaseEntity
    {
        public Guid AccountId { get; set; }
        public Guid BatchId { get; set; }
        public decimal Amount { get; set; }

        // Navigation properties
        public Account Account { get; set; } = null!;
        public IngestionLog IngestionLog { get; set; } = null!;
    }
}
