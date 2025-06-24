using SupermarketAPI.DTOs.Response;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace SupermarketAPI.Services
{
    public interface IProductService
    {
        Task<HomeDto> Home(int? customerId);
        Task<List<ProductDto>> GetTopRatedProductsAsync(int? customerId, int limit);
        Task<Dictionary<string, List<ProductDto>>> GetProductsByCategoryAsync(int? customerId, string slug);
        Task<ProductDetailDto> GetProductDetailsAsync(string slug, int? customerId);
    }
}
