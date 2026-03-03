namespace KreditzBankData.Shared.Data.Entities
{
    /// <summary>
    /// Data-driven categorization rules. All active rules are loaded into IMemoryCache
    /// at startup for O(1) keyword lookup — supports up to 10,000+ rules with no code changes.
    /// </summary>
    public class Categories : BaseEntity
    {
        public string Category { get; set; } = string.Empty;

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
    }
}
