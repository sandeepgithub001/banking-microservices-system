namespace MicroBank.CustomerService.Services.Http;

public interface IAccountServiceClient
{
    Task DeleteAccountsByCustomerAsync(Guid customerId);
}
