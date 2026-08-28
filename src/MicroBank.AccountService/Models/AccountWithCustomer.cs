namespace MicroBank.AccountService.Models;

public class AccountWithCustomer
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string Currency { get; set; } = "USD";
    public decimal Balance { get; set; }
    public DateTime CreatedAt { get; set; }
    public List<Transaction> Transactions { get; set; } = new();
    public Customer Customer { get; set; } = null!;
}

public class Customer
{
    public Guid Id { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string Phone { get; set; } = null!;
}