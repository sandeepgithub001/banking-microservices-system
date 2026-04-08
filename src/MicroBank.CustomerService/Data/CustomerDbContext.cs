using Microsoft.EntityFrameworkCore;
using MicroBank.CustomerService.Models;

namespace MicroBank.CustomerService.Data;

public class CustomerDbContext : DbContext
{
    public CustomerDbContext(DbContextOptions<CustomerDbContext> options) : base(options)
    {
    }

    public DbSet<Customer> Customers => Set<Customer>();
}
