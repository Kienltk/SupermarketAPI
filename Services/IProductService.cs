using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SupermarketAPI.Services
{
    public interface IProductService
    {
        Task<HomeDto> Home(int? customerId);
        Task<List<ProductDto>> GetTopRatedProductsAsync(int? customerId, int limit);
        Task<Dictionary<string, List<ProductDto>>> GetProductsByCategoryAsync(int? customerId, string categorySlug);
        Task<ProductDetailDto> GetProductDetailsAsync(string slug, int? customerId);
        Task<List<ProductDto>> GetProducts(int? customerId);
        Task<List<ProductDto>> GetProductsByProductName(int? customerId, string productName);
        Task<List<ProductDto>> GetProductsByBrand(int? customerId, string brandSlug);
        Task<List<ProductDto>> GetProductsByPrice(int? customerId, decimal minPrice, decimal maxPrice);
        Task<List<ProductDto>> GetProductsByProductNameAndPrice(int? customerId, string productName, decimal minPrice, decimal maxPrice);
        Task<List<ProductDto>> GetProductsByBrandAndCategoryAndRating(int? customerId, string? categorySlug, string? brandSlug, int? ratingScore);
        Task<List<ProductDto>> GetProductbyRatingScore(int? customerId, int ratingscore);
        Task<Product> CreateProductAsync(ProductCreateDto dto);
        Task<Product?> UpdateProductAsync(int id, Product product);
        Task<bool> DeleteProductAsync(int id);
    }
}
