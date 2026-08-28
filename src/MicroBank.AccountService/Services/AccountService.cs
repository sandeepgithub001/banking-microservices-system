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

    public async Task<AccountWithCustomer> GetByIdWithCustomerAsync(Guid id)
    {
        var account = await GetByIdAsync(id);
        var customer = await _customerServiceClient.GetCustomerAsync(account.CustomerId);
        if (customer == null)
        {
            throw new CustomerValidationException("Customer not found.");
        }
        return new AccountWithCustomer
        {
            Id = account.Id,
            CustomerId = account.CustomerId,
            Currency = account.Currency,
            Balance = account.Balance,
            CreatedAt = account.CreatedAt,
            Transactions = account.Transactions,
            Customer = new Customer
            {
                Id = customer.Id,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                Email = customer.Email,
                Phone = customer.Phone
            }
        };
    }

    public async Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _repository.GetByCustomerIdAsync(customerId);
    }

    public async Task DepositAsync(Guid accountId, decimal amount, Guid customerId, string firstName, string lastName, string email)
    {
        if (amount <= 0) throw new ArgumentException("Deposit amount must be positive.", nameof(amount));

        await ValidateCustomerDetails(customerId, firstName, lastName, email);

        var account = await GetByIdAsync(accountId);
        if (account.CustomerId != customerId)
        {
            throw new CustomerValidationException("Account does not belong to the specified customer.");
        }

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

    public async Task WithdrawAsync(Guid accountId, decimal amount, Guid customerId, string firstName, string lastName, string email)
    {
        if (amount <= 0) throw new ArgumentException("Withdrawal amount must be positive.", nameof(amount));

        await ValidateCustomerDetails(customerId, firstName, lastName, email);

        var account = await GetByIdAsync(accountId);
        if (account.CustomerId != customerId)
        {
            throw new CustomerValidationException("Account does not belong to the specified customer.");
        }

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

    private async Task ValidateCustomerDetails(Guid customerId, string firstName, string lastName, string email)
    {
        var customer = await _customerServiceClient.GetCustomerAsync(customerId);
        if (customer == null ||
            customer.FirstName != firstName ||
            customer.LastName != lastName ||
            customer.Email != email)
        {
            throw new CustomerValidationException("Customer details are invalid.");
        }
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
