using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByUsernameAsync(string username);
        Task<Customer> GetCustomerByEmailAsync(string email);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
    }
}
