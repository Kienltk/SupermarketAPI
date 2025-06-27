using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IBrandRepository
    {
        Task<List<Brand>> GetBrandsAsync();
        Task<Brand> GetBrandBySlugAsync(string slug);
    }
}
