namespace MicroBank.AccountService.Exceptions;

public class InsufficientBalanceException : Exception
{
    public InsufficientBalanceException(Guid accountId, decimal requested, decimal available)
        : base($"Withdrawal of {requested:C} failed for account {accountId}; available balance is {available:C}.")
    {
    }
}
