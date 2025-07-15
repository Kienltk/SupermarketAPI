using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IPromotionRepository
    {
        Task CreatePromotionAsync(Promotion promotion);
        Task<List<Promotion>> GetAllPromotionsAsync();
        Task<Promotion> GetPromotionByIdAsync(int promotionId);
        Task UpdatePromotionAsync(Promotion promotion);
        Task AddDiscountAsync(Discount discount);
        Task<Discount> GetDiscountAsync(int productId, int promotionId);
        Task UpdateDiscountAsync(Discount discount);
        Task<Product> GetProductByIdAsync(int productId);
        Task<List<Product>> GetProductsByPromotionIdAsync(int promotionId);
    }
}
