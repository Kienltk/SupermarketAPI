using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly SupermarketContext _context;

        public ProductService(IProductRepository productRepository, SupermarketContext context)
        {
            _productRepository = productRepository;
            _context = context;
        }

        public async Task<HomeDto> Home(int? customerId) {
            var productsByCategory = await GetProductsByCategoryAsync(customerId, string.Empty);
            var topRatedProducts = await GetTopRatedProductsAsync(customerId, 5);

            return new HomeDto
            {
                TopRatedProducts = topRatedProducts,
                ProductyByCategory = productsByCategory
            };
        }

        public async Task<List<ProductDto>> GetTopRatedProductsAsync(int? customerId, int limit)
        {
            var products = await _productRepository.GetTopRatedProductsAsync(limit);
            List<Favorite> favorites = customerId.HasValue
                ? await _productRepository.GetFavoritesByCustomerIdAsync(customerId.Value)
                : new List<Favorite>();

            return products.Select(p => MapToProductDto(p, customerId, favorites)).ToList();
        }

        public async Task<Dictionary<string, List<ProductDto>>> GetProductsByCategoryAsync(int? customerId, string slug)
        {
            int? categoryId = null;
            if (!string.IsNullOrEmpty(slug))
            {
                var category = await _productRepository.GetCategoryBySlugAsync(slug);
                if (category != null)
                {
                    categoryId = category.CategoryId;
                }
            }

            var categories = await _productRepository.GetCategoriesByParentIdAsync(categoryId);
            var categoryProducts = new Dictionary<string, List<ProductDto>>();

            List<Favorite> favorites = customerId.HasValue
                ? await _productRepository.GetFavoritesByCustomerIdAsync(customerId.Value)
                : new List<Favorite>();

            foreach (var category in categories)
            {
                var products = await _productRepository.GetProductsByCategoryIdAsync(category.CategoryId);
                var productDtos = products.Select(p => MapToProductDto(p, customerId, favorites)).ToList();
                categoryProducts.Add(category.CategoryName, productDtos);
            }

            return categoryProducts;
        }

        public async Task<ProductDetailDto> GetProductDetailsAsync(string slug, int? customerId)
        {
            var product = await _productRepository.GetProductBySlugAsync(slug);
            if (product == null)
            {
                return null;
            }

            List<Favorite> favorites = customerId.HasValue
                ? await _productRepository.GetFavoritesByCustomerIdAsync(customerId.Value)
                : new List<Favorite>();

            var productDto = MapToProductDto(product, customerId, favorites);

            var categoryId = await _context.ProductCategories
                .Where(pc => pc.ProductId == product.ProductId)
                .Select(pc => pc.CategoryId)
                .FirstOrDefaultAsync();

            var parentCategory = await _context.Categories
                .Where(c => c.CategoryId == categoryId)
                .Select(c => c.ParentId.HasValue ? _context.Categories.FirstOrDefault(pc => pc.CategoryId == c.ParentId) : c)
                .FirstOrDefaultAsync();

            var relatedProducts = new List<ProductDto>();
            if (parentCategory != null)
            {
                var relatedCategoryProducts = await GetProductsByCategoryAsync(customerId, parentCategory.Slug);
                relatedProducts = relatedCategoryProducts.Values
                    .SelectMany(p => p)
                    .Where(p => p.ProductId != product.ProductId)
                    .Take(16)
                    .ToList();
            }

            return new ProductDetailDto
            {
                ProductDto = productDto,
                RelatedProducts = relatedProducts
            };
        }


        private ProductDto MapToProductDto(Product product, int? customerId, List<Favorite> favorites)
        {
            var currentDate = DateTime.Now;
            var promotion = product.Discounts?.FirstOrDefault()?.Promotion;
            var isFavorite = customerId.HasValue && favorites.Any(f => f.ProductId == product.ProductId);
            Console.WriteLine("IsFavorite:" + isFavorite);

            var avgRating = _context.Ratings
                .Where(r => r.ProductId == product.ProductId)
                .Average(r => (double?)r.RatingScore) ?? 0;

            var response = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Slug = product.Slug,
                Status = product.Status,
                Brand = product.BrandId ?? 0,
                ImageUrl = product.ImageUrl,
                Quantity = product.Quantity,
                isFavorite = isFavorite,
                RatingScore = avgRating,
                PromotionType = promotion?.PromotionType,
                DiscountPercent = promotion?.DiscountPercent,
                DiscountAmount = promotion?.DiscountAmount,
                GiftProductId = promotion?.GiftProductId,
                MinOrderValue = promotion?.MinOrderValue,
                MinOrderQuantity = promotion?.MinOrderQuantity,
                StartDate = promotion?.StartDate ?? DateTime.MinValue,
                EndDate = promotion?.EndDate ?? DateTime.MinValue
            };

            return response;
        }
    }
}
