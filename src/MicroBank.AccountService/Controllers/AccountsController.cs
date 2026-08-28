using Microsoft.AspNetCore.Mvc;
using MicroBank.AccountService.Models;
using MicroBank.AccountService.Services;

namespace MicroBank.AccountService.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;

    public AccountsController(IAccountService accountService)
    {
        _accountService = accountService;
    }

    [HttpGet("customer/{customerId}")]
    public async Task<IActionResult> GetByCustomer(Guid customerId)
    {
        var accounts = await _accountService.GetByCustomerIdAsync(customerId);
        var accountsWithCustomer = new List<AccountWithCustomer>();
        foreach (var account in accounts)
        {
            accountsWithCustomer.Add(await _accountService.GetByIdWithCustomerAsync(account.Id));
        }
        return Ok(accountsWithCustomer);
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> Get(Guid accountId)
    {
        var account = await _accountService.GetByIdWithCustomerAsync(accountId);
        return Ok(account);
    }

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] DepositWithdrawRequest request)
    {
        await _accountService.DepositAsync(request.AccountId, request.Amount, request.CustomerId, request.FirstName, request.LastName, request.Email);
        var account = await _accountService.GetByIdWithCustomerAsync(request.AccountId);
        return Ok(account);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] DepositWithdrawRequest request)
    {
        await _accountService.WithdrawAsync(request.AccountId, request.Amount, request.CustomerId, request.FirstName, request.LastName, request.Email);
        var account = await _accountService.GetByIdWithCustomerAsync(request.AccountId);
        return Ok(account);
    }

    [HttpDelete("{accountId}")]
    public async Task<IActionResult> Delete(Guid accountId)
    {
        await _accountService.DeleteAccountAsync(accountId);
        return NoContent();
    }

    [HttpDelete("customer/{customerId}")]
    public async Task<IActionResult> DeleteByCustomer(Guid customerId)
    {
        await _accountService.DeleteAccountsByCustomerAsync(customerId);
        return NoContent();
    }
}

public record DepositWithdrawRequest(Guid AccountId, decimal Amount, Guid CustomerId, string FirstName, string LastName, string Email);
