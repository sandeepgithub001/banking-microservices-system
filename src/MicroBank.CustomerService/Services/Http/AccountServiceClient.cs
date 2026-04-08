using System.Net;
using System.Net.Http.Json;

namespace MicroBank.CustomerService.Services.Http;

public class AccountServiceClient : IAccountServiceClient
{
    private readonly HttpClient _httpClient;

    public AccountServiceClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task DeleteAccountsByCustomerAsync(Guid customerId)
    {
        var response = await _httpClient.DeleteAsync($"/api/accounts/customer/{customerId}");
        if (!response.IsSuccessStatusCode && response.StatusCode != HttpStatusCode.NotFound)
        {
            var content = await response.Content.ReadAsStringAsync();
            throw new Exception($"Failed to delete customer accounts: {response.StatusCode} {content}");
        }
    }
}
