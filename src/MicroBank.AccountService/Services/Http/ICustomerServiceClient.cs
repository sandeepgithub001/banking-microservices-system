namespace MicroBank.AccountService.Services.Http;

public interface ICustomerServiceClient
{
    Task ValidateCustomerAsync(Guid customerId);
    Task<CustomerDto?> GetCustomerAsync(Guid customerId);
}

public record CustomerDto(Guid Id, string FirstName, string LastName, string Email, string Phone);
