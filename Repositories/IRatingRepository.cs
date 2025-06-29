namespace SupermarketAPI.Repositories
{
    public interface IRatingRepository
    {
        double GetAvgRatingProduct(int productId);
    }
}
