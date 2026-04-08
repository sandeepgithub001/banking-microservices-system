namespace MicroBank.AccountService.Services.Http;

public interface ICustomerServiceClient
{
    Task ValidateCustomerAsync(Guid customerId);
}
