using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<List<RatingDto>> GetRatingsByProductIdAsync(int productId)
        {
            var ratings = await _ratingRepository.GetRatingsByProductIdAsync(productId);
            return ratings.Select(r => new RatingDto
            {
                RatingId = r.RatingId,
                RatingScore = r.RatingScore,
                Comment = r.Comment,
                CustomerName = r.Customer.FirstName + " " + r.Customer.LastName,
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