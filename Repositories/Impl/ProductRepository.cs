using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class ProductRepository : IProductRepository
    {
        private readonly SupermarketContext _context;

        public ProductRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task<Category> GetCategoryBySlugAsync(string slug)
        {
            return await _context.Categories.FirstOrDefaultAsync(c => c.Slug == slug);
        }

        public async Task<List<Category>> GetCategoriesByParentIdAsync(int? parentId)
        {
            return await _context.Categories
                .Where(c => c.ParentId == parentId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Include(pc => pc.Product)
                .ThenInclude(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .Select(pc => pc.Product)
                .ToListAsync();
        }

        public async Task<Product> GetProductBySlugAsync(string slug)
        {
            return await _context.Products
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .FirstOrDefaultAsync(p => p.Slug == slug);
        }

        public async Task<List<Favorite>> GetFavoritesByCustomerIdAsync(int customerId)
        {
            return await _context.Favorites
                .Where(f => f.CustomerId == customerId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetTopRatedProductsAsync(int limit)
        {
            return await _context.Products
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .GroupJoin(_context.Ratings,
                    p => p.ProductId,
                    r => r.ProductId,
                    (p, ratings) => new { Product = p, AvgRating = ratings.Average(r => (double?)r.RatingScore) ?? 0 })
                .OrderByDescending(x => x.AvgRating)
                .Take(limit)
                .Select(x => x.Product)
                .ToListAsync();
        }
    }
}
