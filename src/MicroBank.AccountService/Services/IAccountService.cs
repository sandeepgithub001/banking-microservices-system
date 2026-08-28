using MicroBank.AccountService.Models;

namespace MicroBank.AccountService.Services;

public interface IAccountService
{
    Task<Account> GetByIdAsync(Guid id);
    Task<AccountWithCustomer> GetByIdWithCustomerAsync(Guid id);
    Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId);
    Task DepositAsync(Guid accountId, decimal amount, Guid customerId, string firstName, string lastName, string email);
    Task WithdrawAsync(Guid accountId, decimal amount, Guid customerId, string firstName, string lastName, string email);
    Task DeleteAccountAsync(Guid accountId);
    Task DeleteAccountsByCustomerAsync(Guid customerId);
}
