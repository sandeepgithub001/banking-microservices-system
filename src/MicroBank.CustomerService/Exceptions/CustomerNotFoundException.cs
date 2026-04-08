namespace MicroBank.CustomerService.Exceptions;

public class CustomerNotFoundException : Exception
{
    public CustomerNotFoundException(Guid customerId)
        : base($"Customer with id '{customerId}' was not found.")
    {
    }
}
