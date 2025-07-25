using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _productRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly IBrandRepository _brandRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IPromotionRepository _promotionRepository;
        private readonly SupermarketContext _context;

        public ProductService(IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IFavoriteRepository favoriteRepository,
            IBrandRepository brandRepository,
            IRatingRepository ratingRepository,
            IPromotionRepository promotionRepository,
            SupermarketContext context)
        {
            _context = context;
            _productRepository = productRepository;
            _categoryRepository = categoryRepository;
            _favoriteRepository = favoriteRepository;
            _brandRepository = brandRepository;
            _ratingRepository = ratingRepository;
            _promotionRepository = promotionRepository;
        }

        public async Task<HomeDto> Home(int? customerId)
        {
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

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<Dictionary<string, List<ProductDto>>> GetProductsByCategoryAsync(int? customerId, string categorySlug)
        {
            int? categoryId = null;
            var categoryProducts = new Dictionary<string, List<ProductDto>>();
            var uniqueProductIds = new HashSet<int>();

            if (!string.IsNullOrEmpty(categorySlug))
            {
                var category = await _categoryRepository.GetCategoryBySlugAsync(categorySlug);
                if (category != null)
                {
                    categoryId = category.CategoryId;
                    var products = await _productRepository.GetProductsByCategoryIdAsync(category.CategoryId);
                    var productDtos = products
                        .Where(p => !uniqueProductIds.Contains(p.ProductId))
                        .Select(p => MapToProductDto(p, customerId))
                        .ToList();
                    if (productDtos.Any())
                    {
                        categoryProducts.Add(category.CategoryName, productDtos);
                        uniqueProductIds.UnionWith(products.Select(p => p.ProductId));
                    }
                }
            }

            var categories = await _categoryRepository.GetCategoriesByParentIdAsync(categoryId);

            foreach (var category in categories)
            {
                var products = await _productRepository.GetProductsByCategoryIdAsync(category.CategoryId);
                var productDtos = products
                    .Where(p => !uniqueProductIds.Contains(p.ProductId))
                    .Select(p => MapToProductDto(p, customerId))
                    .ToList();
                if (productDtos.Any())
                {
                    categoryProducts.Add(category.CategoryName, productDtos);
                    uniqueProductIds.UnionWith(products.Select(p => p.ProductId));
                }
            }

            return categoryProducts;
        }

        public async Task<ProductDetailDto> GetProductDetailsAsync(string slug, int? customerId)
        {
            var product = await _productRepository.GetProductBySlugAsync(slug);
            if (product == null)
            {
                throw new EntryPointNotFoundException("Not found product");
            }

            var productDto = MapToProductDto(product, customerId);
            var ratings = await _ratingRepository.GetRatingsByProductIdAsync(product.ProductId);
            var ratingDtos = ratings.Select(r => new RatingDto
            {
                RatingId = r.RatingId,
                RatingScore = r.RatingScore,
                CustomerId = r.CustomerId,
                Comment = r.Comment,
                CustomerName = string.Join(" ", new[] { r.Customer.FirstName, r.Customer.MiddleName, r.Customer.LastName }
                                     .Where(s => !string.IsNullOrWhiteSpace(s))),
                CreatedAt = r.CreatedAt
            }).ToList();

            var categoryId = _categoryRepository.GetCategoryIdByProductId(product.ProductId);

            var parentCategory = _categoryRepository.GetParentCategoryByCategoryId(categoryId);

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

            List<CategoryDto> categories = _categoryRepository.GetCategoriesByProductId(product.ProductId)
                .Result
                .Select(c => new CategoryDto
                {
                    Id = c.CategoryId,
                    CategoryName = c.CategoryName,
                    slug = c.Slug,
                }).ToList();

            return new ProductDetailDto
            {
                ProductDto = productDto,
                RelatedProducts = relatedProducts,
                Ratings = ratingDtos,
                Categories = categories
            };
        }

        public async Task<List<ProductDto>> GetProducts(int? customerId)
        {
            var products = await _productRepository.GetProductsAsync();

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<List<ProductDto>> GetProductsByProductName(int? customerId, string productName)
        {
            var products = await _productRepository.GetProductsByProductNameAsync(productName);

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<List<ProductDto>> GetProductsByBrand(int? customerId, string brandSlug)
        {
            var brandId = _brandRepository.GetBrandBySlugAsync(brandSlug).Result.BrandId;

            var products = await _productRepository.GetProductsByBrandIdAsync(brandId);

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<List<ProductDto>> GetProductsByPrice(int? customerId, decimal minPrice, decimal maxPrice)
        {
            var products = await _productRepository.GetProductsByPriceAsync(minPrice, maxPrice);

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<List<ProductDto>> GetProductsByProductNameAndPrice(int? customerId, string productName, decimal minPrice, decimal maxPrice)
        {
            var products = await _productRepository.GetProductsByProductNameAndPrice(productName, minPrice, maxPrice);

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<List<ProductDto>> GetProductsByBrandAndCategoryAndRating(int? customerId, string? category, string? brand, int? ratingScore)
        {
            int? categoryId = null;
            int? brandId = null;

            if (!string.IsNullOrEmpty(category))
            {
                categoryId = _categoryRepository.GetCategoryBySlugAsync(category).Result.CategoryId;
            }

            if (!string.IsNullOrEmpty(brand)) { 
                brandId = _brandRepository.GetBrandBySlugAsync(brand).Result.BrandId;
            }

            var products = await _productRepository.GetProductsByBrandAndCategoryAndRating(categoryId, brandId, ratingScore);

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public async Task<List<ProductDto>> GetProductbyRatingScore(int? customerId, int ratingscore)
        {
            var products = await _productRepository.GetProductsByRatingScore(ratingscore);

            return products.Select(p => MapToProductDto(p, customerId)).ToList();
        }

        public ProductDto MapToProductDto(Product product, int? customerId)
        {
            var currentDate = DateTime.Now;
            var promotion = product.Discounts?.FirstOrDefault()?.Promotion;
            List<Favorite> favorites = customerId.HasValue
                ? _favoriteRepository.GetFavoritesByCustomerIdAsync(customerId.Value)
                : new List<Favorite>();
            var isFavorite = favorites.Any(f => f.ProductId == product.ProductId);
            var brand = product.Brand?.BrandName;
            var categoryId = _categoryRepository.GetCategoryIdByProductId(product.ProductId);
            var avgRating = _ratingRepository.GetAvgRatingProduct(product.ProductId);

            var response = new ProductDto
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                Price = product.Price,
                Slug = product.Slug,
                Status = product.Status,
                Brand = brand,
                ImageUrl = product.ImageUrl,
                Quantity = product.Quantity,
                isFavorite = isFavorite,
                RatingScore = avgRating,
                PromotionType = promotion?.PromotionType,
                PromotionDescription = promotion?.Description,
                DiscountPercent = promotion?.DiscountPercent,
                DiscountAmount = promotion?.DiscountAmount,
                GiftProductId = promotion?.GiftProductId,
                GiftProductName = promotion?.GiftProduct?.ProductName,
                GiftProductSlug = promotion?.GiftProduct?.Slug,
                GiftProductImg = promotion?.GiftProduct?.ImageUrl,
                GiftProductPrice = promotion?.GiftProduct?.Price,
                MinOrderValue = promotion?.MinOrderValue,
                MinOrderQuantity = promotion?.MinOrderQuantity,
                StartDate = promotion?.StartDate ?? DateTime.MinValue,
                CategoryId = categoryId,
                EndDate = promotion?.EndDate ?? DateTime.MinValue
            };

            return response;
        }

        public async Task<Product> CreateProductAsync(ProductCreateDto dto)
        {
            var existingProduct = await _context.Products
                .FirstOrDefaultAsync(p => p.ProductName == dto.ProductName || p.Slug == dto.Slug);

            if (existingProduct != null)
            {
                throw new Exception("Product name or slug already exists.");
            }

            var TotalAmount = dto.UnitCost * dto.Quantity;

            var product = new Product
            {
                ProductName = dto.ProductName,
                Price = dto.Price,
                Slug = dto.Slug,
                Status = dto.Status,
                Quantity = dto.Quantity,
                BrandId = dto.BrandId,
                ImageUrl = dto.ImageUrl,
                UnitCost = dto.UnitCost,
                TotalAmount = TotalAmount
            };

            if (dto.CategoryId != null && dto.CategoryId.Any())
            {
                var categories = await _context.Categories
                    .Where(c => dto.CategoryId.Contains(c.CategoryId))
                    .ToListAsync();

                product.ProductCategories = categories.Select(c => new ProductCategory
                {
                    CategoryId = c.CategoryId
                }).ToList();
            }

            _context.Products.Add(product);
            await _context.SaveChangesAsync();


            if (dto.PromotionId != null)
            {
                var newProduct = _productRepository.GetProductBySlugAsync(product.Slug);
                var discount = new Discount
                {
                    ProductId = newProduct.Result.ProductId,
                    PromotionId = dto.PromotionId,
                    IsActive = true
                };
                _promotionRepository.AddDiscountAsync(discount);
            }
            return product;
        }


        public async Task<bool> DeleteProductAsync(int id)
        {
            return await _productRepository.DeleteProductAsync(id);
        }

        async Task<ProductDto?> IProductService.UpdateProductAsync(int id, ProductUpdateDto productUpdate)
        {
            var existingProduct = await _context.Products
                .Include(p => p.ProductCategories)
                .Include(p => p.Promotions)
                .Include(p => p.Brand)
                .FirstOrDefaultAsync(p => p.ProductId == id);

            if (existingProduct == null) return null;

            var TotalAmount = productUpdate.UnitCost ?? existingProduct.UnitCost * productUpdate.Quantity ?? existingProduct.Quantity;

            existingProduct.ProductName = productUpdate.ProductName ?? existingProduct.ProductName;
            existingProduct.Price = productUpdate.Price ?? existingProduct.Price;
            existingProduct.Slug = productUpdate.Slug ?? existingProduct.Slug;
            existingProduct.Status = productUpdate.Status ?? existingProduct.Status;
            existingProduct.Quantity = productUpdate.Quantity ?? existingProduct.Quantity;
            existingProduct.BrandId = productUpdate.BrandId ?? existingProduct.BrandId;
            existingProduct.ImageUrl = productUpdate.ImageUrl ?? existingProduct.ImageUrl;
            existingProduct.UnitCost = productUpdate.UnitCost ?? existingProduct.UnitCost;
            existingProduct.TotalAmount = TotalAmount;

            if (productUpdate.PromotionId != null)
            {
                var promotions = await _context.Promotions
                    .Where(p => productUpdate.PromotionId.Contains(p.PromotionId))
                    .ToListAsync();
                existingProduct.Promotions = promotions;
            }

            if (productUpdate.CategoryId != null)
            {
                var categories = await _context.Categories
                    .Where(c => productUpdate.CategoryId.Contains(c.CategoryId))
                    .ToListAsync();

                existingProduct.ProductCategories = categories
                    .Select(c => new ProductCategory
                    {
                        ProductId = existingProduct.ProductId,
                        CategoryId = c.CategoryId
                    }).ToList();
            }

            await _context.SaveChangesAsync();

            return MapToProductDto(existingProduct, null);
        }

    }
}
