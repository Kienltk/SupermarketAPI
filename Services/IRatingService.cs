using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface IRatingService
    {
        Task<List<RatingDto>> GetRatingsByProductSlugAsync(string productSlug);
        Task CreateRatingAsync(int customerId, RatingCreateDto dto);
        Task UpdateRatingAsync(int ratingId, int customerId, RatingUpdateDto dto, bool isAdmin);
        Task DeleteRatingAsync(int ratingId, int customerId, bool isAdmin);
    }
}
