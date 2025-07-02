using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface ICustomerRepository
    {
        Task<Customer> GetCustomerByUsernameAsync(string username);
        Task AddCustomerAsync(Customer customer);
        Task UpdateCustomerAsync(Customer customer);
    }
}
