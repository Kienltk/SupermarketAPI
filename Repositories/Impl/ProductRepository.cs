using System.Collections.Generic;
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

        public async Task<Product> GetProductByIdAsync(int id)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .FirstAsync(p => p.ProductId == id);
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
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
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

        public async Task<List<Product>> GetProductsByBrandAndCategoryAndRating(int? categoryId, int? brandId, int? ratingScore)
        {
            var query = _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .AsQueryable();

            if (brandId.HasValue)
            {
                query = query.Where(p => p.BrandId == brandId.Value);
            }

            if (categoryId.HasValue)
            {
                query = query.Where(p => p.ProductCategories.Any(pc => pc.CategoryId == categoryId.Value));
            }

            if (ratingScore.HasValue)
            {
                var ratedProducts = _context.Ratings
                    .GroupBy(r => r.ProductId)
                    .Select(g => new
                    {
                        ProductId = g.Key,
                        AvgRating = g.Average(r => (double?)r.RatingScore) ?? 0
                    });

                if (ratingScore < 5)
                {
                    query = query.Where(p =>
                        ratedProducts.Any(rp => rp.ProductId == p.ProductId && rp.AvgRating >= ratingScore));
                }
                else
                {
                    query = query.Where(p =>
                        ratedProducts.Any(rp => rp.ProductId == p.ProductId && rp.AvgRating == ratingScore));
                }
            }

            return await query
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion).ToListAsync(); ;
        }

        public async Task<List<Product>> GetProductsByRatingScore(int ratingScore)
        {
            return await _context.Products
                .Include(p => p.Brand)
                .Include(p => p.Discounts)
                .ThenInclude(d => d.Promotion)
                .GroupJoin(_context.Ratings,
                    p => p.ProductId,
                    r => r.ProductId,
                    (p, ratings) => new { Product = p, AvgRating = ratings.Average(r => (double?)r.RatingScore) ?? 0 })
                .Where(x => ratingScore < 5? x.AvgRating >= ratingScore : x.AvgRating == ratingScore)
                .Select(x => x.Product)
                .ToListAsync();
        }

    }
}
