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

    [HttpPost("deposit")]
    public async Task<IActionResult> Deposit([FromBody] TransactionRequest request)
    {
        await _accountService.DepositAsync(request.AccountId, request.Amount);
        var account = await _accountService.GetByIdAsync(request.AccountId);
        return Ok(account);
    }

    [HttpPost("withdraw")]
    public async Task<IActionResult> Withdraw([FromBody] TransactionRequest request)
    {
        await _accountService.WithdrawAsync(request.AccountId, request.Amount);
        var account = await _accountService.GetByIdAsync(request.AccountId);
        return Ok(account);
    }

    [HttpGet("{accountId}")]
    public async Task<IActionResult> Get(Guid accountId)
    {
        var account = await _accountService.GetByIdAsync(accountId);
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

public record TransactionRequest(Guid AccountId, decimal Amount);
