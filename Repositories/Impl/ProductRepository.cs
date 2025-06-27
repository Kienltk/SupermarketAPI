using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs.Response;
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

        public async Task<List<Product>> GetProductsByCategoryIdAsync(int categoryId)
        {
            return await _context.ProductCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Include(pc => pc.Product)
                .ThenInclude(p => p.Brand)
                .Include(pc => pc.Product.Discounts)
                .ThenInclude(d => d.Promotion)
                .Select(pc => pc.Product)
                .ToListAsync();
        }

        public async Task<Product> GetProductBySlugAsync(string slug)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .FirstAsync(p => p.Slug == slug);
        }

        public async Task<List<Product>> GetTopRatedProductsAsync(int limit)
        {
            return await _context.Products
                .Include(p => p.Brand)
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

        public async Task<List<Product>> GetProductsAsync()
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .ToListAsync();
        }

        public double GetAvgRatingProduct(int productId)
        {
            return _context.Ratings
                .Where(r => r.ProductId == productId)
                .Average(r => (double?)r.RatingScore) ?? 0;
        }

        public async Task<List<Product>> GetProductsByProductNameAsync(string productName)
        {
            return await _context.Products
                .Where(p => p.ProductName.Contains(productName))
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByBrandIdAsync(int brandId)
        {
            return await _context.Products
                .Where(p => p.BrandId == brandId)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByPriceAsync(decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.Price >= minPrice && p.Price <= maxPrice)
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .ToListAsync();
        }

        public async Task<List<Product>> GetProductsByProductNameAndPrice(string productName, decimal minPrice, decimal maxPrice)
        {
            return await _context.Products
                .Where(p => p.ProductName.Contains(productName) && p.Price >= minPrice && p.Price <= maxPrice)
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .ToListAsync();
        }
    }
}
