namespace MicroBank.AccountService.Models;

public class Transaction
{
    public Guid Id { get; set; }
    public Guid AccountId { get; set; }
    public decimal Amount { get; set; }
    public string Type { get; set; } = null!;
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
