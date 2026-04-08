using MicroBank.AccountService.Models;

namespace MicroBank.AccountService.Services;

public interface IAccountService
{
    Task<Account> GetByIdAsync(Guid id);
    Task DepositAsync(Guid accountId, decimal amount);
    Task WithdrawAsync(Guid accountId, decimal amount);
    Task DeleteAccountAsync(Guid accountId);
    Task DeleteAccountsByCustomerAsync(Guid customerId);
}
