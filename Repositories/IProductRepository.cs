using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IProductRepository
    {
        Task<List<Product>> GetProductsAsync();
        Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId);
        Task<Product> GetProductBySlugAsync(string slug);
        Task<List<Product>> GetTopRatedProductsAsync(int limit);
        double GetAvgRatingProduct(int productId);
        Task<List<Product>> GetProductsByProductNameAsync(string productName);
        Task<List<Product>> GetProductsByBrandIdAsync(int brandId);
        Task<List<Product>> GetProductsByPriceAsync(decimal minPrice, decimal maxPrice);
        Task<List<Product>> GetProductsByProductNameAndPrice(string productName, decimal minPrice, decimal maxPrice);

    }
}
