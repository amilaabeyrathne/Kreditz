namespace KreditzBankData.Shared.Data.Entities
{
    public class AccountHolder : BaseEntity
    {
        public Guid AccountId { get; set; }
        public string Name { get; set; } = string.Empty;

        // Navigation properties
        public Account Account { get; set; } = null!;
    }
}
