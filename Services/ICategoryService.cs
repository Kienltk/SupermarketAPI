using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;

namespace SupermarketAPI.Services
{
    public interface ICategoryService
    {
        Task<List<CategoriesDto>> GetCategories();
        Task<CategoryDto> CreateCategoryAsync(CategoryDto dto);
        Task<CategoryDto> UpdateCategoryAsync(int id, CategoryDto dto);
        Task DeleteCategoryAsync(int id);
        Task<CategoryDto> GetCategoryByIdAsync(int id);

    }
}
