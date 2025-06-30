using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using SupermarketAPI.Repositories.Impl;

namespace SupermarketAPI.Services.Impl
{
    public class FavoriteService : IFavoriteService
    {
        private readonly IFavoriteRepository _favoriteRepository;
        private readonly ProductService _productService;

        public FavoriteService(IFavoriteRepository favoriteRepository, ProductService productService)
        {
            _favoriteRepository = favoriteRepository;
            _productService = productService;
        }

        public Task<List<ProductDto>> GetFavoritesProductByUserId(int customerId)
        {
            var favorites = _favoriteRepository.GetFavoritesByCustomerIdAsync(customerId);

            if (favorites == null)
            {
                throw new InvalidOperationException("Favorites not found");
            }

            var favoriteProducts = new List<ProductDto>();

            if (favorites != null)
            {
                foreach (Favorite favorite in favorites)
                {
                    if (favorite?.Product != null)
                    {
                        ProductDto product = _productService.MapToProductDto(favorite.Product, customerId);
                        favoriteProducts.Add(product);
                    }
                }
            }

            return Task.FromResult(favoriteProducts);
        }

        public async Task<bool> AddFavorite(int customerId, int productId)
        {
            var existingFavorite = await _favoriteRepository.GetFavoriteAsync(customerId, productId);
            if (existingFavorite != null)
            {
                return false;
            }

            //Console.WriteLine("CustomerId: " + customerId + ", productId: " + productId);

            var favorite = new Favorite
            {
                CustomerId = customerId,
                ProductId = productId
            };

            await _favoriteRepository.AddFavoriteAsync(favorite);
            return true;
        }

        public async Task<bool> DeleteFavorite(int customerId, int productId)
        {
            var existingFavorite = await _favoriteRepository.GetFavoriteAsync(customerId, productId);
            if (existingFavorite == null)
            {
                return false;
            }

            await _favoriteRepository.DeleteFavoriteAsync(existingFavorite);
            return true;
        }
    }
}
