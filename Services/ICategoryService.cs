using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public interface ICategoryService
    {
        Task<List<CategoriesDto>> GetCategories();
    }
}
