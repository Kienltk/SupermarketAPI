using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface ICategoryRepository
    {
        Task<Category> GetCategoryBySlugAsync(string slug);
        Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId);
        Task<List<Category>> GetCategoriesAsync();
        int GetCategoryIdByProductId(int productId);
        Task<List<Category>> GetCategoriesByProductId(int productId);
        Category GetParentCategoryByCategoryId(int categoryId);
    }
}
