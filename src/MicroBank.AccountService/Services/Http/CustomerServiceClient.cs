using System.Net;
using System.Net.Http.Json;
using MicroBank.AccountService.Exceptions;

namespace MicroBank.AccountService.Services.Http;

public class CustomerServiceClient : ICustomerServiceClient
{
    private readonly HttpClient _httpClient;

    public CustomerServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task ValidateCustomerAsync(Guid customerId)
    {
        var response = await _httpClient.GetAsync($"/api/customers/{customerId}");
        if (!response.IsSuccessStatusCode)
        {
            if (response.StatusCode == HttpStatusCode.NotFound)
            {
                throw new CustomerValidationException(customerId);
            }

            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Customer validation call failed: {response.StatusCode} {content}");
        }
    }

    public async Task<CustomerDto?> GetCustomerAsync(Guid customerId)
    {
        var response = await _httpClient.GetAsync($"/api/customers/{customerId}");
        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<CustomerDto>();
        }
        return null;
    }
}
