using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetBrands();
    }
}
