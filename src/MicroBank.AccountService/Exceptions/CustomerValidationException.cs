namespace MicroBank.AccountService.Exceptions;

public class CustomerValidationException : Exception
{
    public CustomerValidationException(Guid customerId)
        : base($"Customer validation failed for customer id '{customerId}'.")
    {
    }

    public CustomerValidationException(string message)
        : base(message)
    {
    }
}
