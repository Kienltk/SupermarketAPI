using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface IBrandService
    {
        Task<List<BrandDto>> GetBrands();
        Task<BrandDto> GetBrandByIdAsync(int id);
        Task<BrandDto> CreateBrandAsync(BrandDto dto);
        Task<BrandDto> UpdateBrandAsync(int id, BrandDto dto);
        Task DeleteBrandAsync(int id);
    }
}
