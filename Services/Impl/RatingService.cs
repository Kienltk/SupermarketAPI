using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IProductRepository _productRepository;

        public RatingService(
            IRatingRepository ratingRepository,
            IProductRepository productRepository)
        {
            _ratingRepository = ratingRepository;
            _productRepository = productRepository;
        }

        public async Task<List<RatingDto>> GetRatingsByProductSlugAsync(string productSlug)
        {
            int productId = _productRepository.GetProductBySlugAsync(productSlug).Result.ProductId;
            var ratings = await _ratingRepository.GetRatingsByProductIdAsync(productId);
            return ratings.Select(r => new RatingDto
            {
                RatingId = r.RatingId,
                RatingScore = r.RatingScore,
                CustomerId = r.CustomerId,
                Comment = r.Comment,
                CustomerName = string.Join(" ", new[] { r.Customer.FirstName, r.Customer.MiddleName, r.Customer.LastName }
                                     .Where(s => !string.IsNullOrWhiteSpace(s))),
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task CreateRatingAsync(int customerId, RatingCreateDto dto)
        {
            var rating = new Rating
            {
                CustomerId = customerId,
                ProductId = dto.ProductId,
                RatingScore = dto.RatingScore,
                Comment = dto.Comment,
                CreatedAt = DateTime.UtcNow
            };
            await _ratingRepository.CreateRatingAsync(rating);
        }

        public async Task UpdateRatingAsync(int ratingId, int customerId, RatingUpdateDto dto, bool isAdmin)
        {
            var rating = await _ratingRepository.GetRatingByIdAsync(ratingId);
            if (rating == null)
                throw new KeyNotFoundException("Rating not found");

            if (!isAdmin && rating.CustomerId != customerId)
                throw new UnauthorizedAccessException("You are not authorized to update this rating");

            rating.RatingScore = dto.RatingScore;
            rating.Comment = dto.Comment;

            await _ratingRepository.UpdateRatingAsync(rating);
        }

        public async Task DeleteRatingAsync(int ratingId, int customerId, bool isAdmin)
        {
            var rating = await _ratingRepository.GetRatingByIdAsync(ratingId);
            if (rating == null)
                throw new KeyNotFoundException("Rating not found");

            if (!isAdmin && rating.CustomerId != customerId)
                throw new UnauthorizedAccessException("You are not authorized to delete this rating");

            await _ratingRepository.DeleteRatingAsync(ratingId);
        }
    }
}