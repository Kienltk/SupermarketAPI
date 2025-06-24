using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IProductRepository
    {
        Task<Category> GetCategoryBySlugAsync(string slug);
        Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId);
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Product> GetProductBySlugAsync(string slug);
        Task<List<Favorite>> GetFavoritesByCustomerIdAsync(int customerId);
        Task<List<Product>> GetTopRatedProductsAsync(int limit);
    }
}
