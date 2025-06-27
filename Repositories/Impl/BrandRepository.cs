using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class BrandRepository : IBrandRepository
    {
        private readonly SupermarketContext _context;

        public BrandRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task<List<Brand>> GetBrandsAsync()
        {
            return await _context.Brands.ToListAsync();
        }

        public async Task<Brand> GetBrandBySlugAsync(string slug)
        {
            return await _context.Brands.FirstAsync(b => b.Slug == slug);
        }
    }
}
