using Microsoft.EntityFrameworkCore;
using MicroBank.AccountService.Data;
using MicroBank.AccountService.Models;

namespace MicroBank.AccountService.Repositories;

public class AccountRepository : IAccountRepository
{
    private readonly AccountDbContext _dbContext;

    public AccountRepository(AccountDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Account?> GetByIdAsync(Guid id)
    {
        return await _dbContext.Accounts
            .Include(a => a.Transactions)
            .SingleOrDefaultAsync(a => a.Id == id);
    }

    public async Task<IEnumerable<Account>> GetByCustomerIdAsync(Guid customerId)
    {
        return await _dbContext.Accounts
            .Include(a => a.Transactions)
            .Where(a => a.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task AddAsync(Account account)
    {
        await _dbContext.Accounts.AddAsync(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task UpdateAsync(Account account)
    {
        _dbContext.Accounts.Update(account);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(Account account)
    {
        _dbContext.Accounts.Remove(account);
        await _dbContext.SaveChangesAsync();
    }
}
