using MicroBank.CustomerService.Models;

namespace MicroBank.CustomerService.Services;

public interface ICustomerService
{
    Task<IEnumerable<Customer>> GetAllAsync();
    Task<Customer> GetByIdAsync(Guid id);
    Task<Customer> CreateAsync(Customer customer);
    Task UpdateAsync(Guid id, Customer customer);
    Task DeleteAsync(Guid id);
}
