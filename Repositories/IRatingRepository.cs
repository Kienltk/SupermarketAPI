using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IRatingRepository
    {
        double GetAvgRatingProduct(int productId);

        Task<List<Rating>> GetRatingsByProductIdAsync(int productId);

        Task<Rating?> GetRatingByIdAsync(int ratingId);

        Task CreateRatingAsync(Rating rating);

        Task UpdateRatingAsync(Rating rating);

        Task DeleteRatingAsync(int ratingId);
    }
}
