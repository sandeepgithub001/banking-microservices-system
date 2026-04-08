using MicroBank.AccountService.Exceptions;
using MicroBank.AccountService.Models;
using MicroBank.AccountService.Repositories;
using MicroBank.AccountService.Services.Http;

namespace MicroBank.AccountService.Services;

public class AccountService : IAccountService
{
    private readonly IAccountRepository _repository;
    private readonly ICustomerServiceClient _customerServiceClient;

    public AccountService(IAccountRepository repository, ICustomerServiceClient customerServiceClient)
    {
        _repository = repository;
        _customerServiceClient = customerServiceClient;
    }

    public async Task<Account> GetByIdAsync(Guid id)
    {
        var account = await _repository.GetByIdAsync(id);
        return account ?? throw new AccountNotFoundException(id);
    }

    public async Task DepositAsync(Guid accountId, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        var account = await GetByIdAsync(accountId);
        account.Balance += amount;
        account.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = amount,
            Type = "Deposit",
            Timestamp = DateTime.UtcNow
        });
        await _repository.UpdateAsync(account);
    }

    public async Task WithdrawAsync(Guid accountId, decimal amount)
    {
        if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));

        var account = await GetByIdAsync(accountId);
        if (account.Balance < amount)
        {
            throw new InsufficientBalanceException(accountId, amount, account.Balance);
        }

        account.Balance -= amount;
        account.Transactions.Add(new Transaction
        {
            Id = Guid.NewGuid(),
            AccountId = account.Id,
            Amount = amount,
            Type = "Withdrawal",
            Timestamp = DateTime.UtcNow
        });
        await _repository.UpdateAsync(account);
    }

    public async Task DeleteAccountAsync(Guid accountId)
    {
        var account = await GetByIdAsync(accountId);
        await _repository.DeleteAsync(account);
    }

    public async Task DeleteAccountsByCustomerAsync(Guid customerId)
    {
        var accounts = await _repository.GetByCustomerIdAsync(customerId);
        foreach (var account in accounts)
        {
            await _repository.DeleteAsync(account);
        }
    }
}
