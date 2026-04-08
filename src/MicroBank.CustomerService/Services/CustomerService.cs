using MicroBank.CustomerService.Exceptions;
using MicroBank.CustomerService.Models;
using MicroBank.CustomerService.Repositories;
using MicroBank.CustomerService.Services.Http;

namespace MicroBank.CustomerService.Services;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _repository;
    private readonly IAccountServiceClient _accountServiceClient;

    public CustomerService(ICustomerRepository repository, IAccountServiceClient accountServiceClient)
    {
        _repository = repository;
        _accountServiceClient = accountServiceClient;
    }

    public async Task<IEnumerable<Customer>> GetAllAsync() => await _repository.GetAllAsync();

    public async Task<Customer> GetByIdAsync(Guid id)
    {
        var customer = await _repository.GetByIdAsync(id);
        return customer ?? throw new CustomerNotFoundException(id);
    }

    public async Task<Customer> CreateAsync(Customer customer)
    {
        customer.Id = Guid.NewGuid();
        customer.CreatedAt = DateTime.UtcNow;
        await _repository.AddAsync(customer);
        return customer;
    }

    public async Task UpdateAsync(Guid id, Customer customer)
    {
        var existing = await _repository.GetByIdAsync(id) ?? throw new CustomerNotFoundException(id);

        existing.FirstName = customer.FirstName;
        existing.LastName = customer.LastName;
        existing.Email = customer.Email;
        existing.Phone = customer.Phone;

        await _repository.UpdateAsync(existing);
    }

    public async Task DeleteAsync(Guid id)
    {
        var existing = await _repository.GetByIdAsync(id) ?? throw new CustomerNotFoundException(id);
        await _accountServiceClient.DeleteAccountsByCustomerAsync(id);
        await _repository.DeleteAsync(existing);
    }
}
