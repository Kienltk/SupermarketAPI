using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SupermarketAPI.Services
{
    public interface IPromotionService
    {
        Task<PromotionDto> CreatePromotionAsync(PromotionDto promotionRequest);
        Task<List<PromotionDto>> GetAllPromotionsAsync();
        Task<PromotionDto> GetPromotionByIdAsync(int promotionId);
        Task<bool> UpdatePromotionAsync(int promotionId, bool isActive);
        Task<bool> AddProductToPromotionAsync(int productId, int promotionId);
        Task<bool> UpdateProductPromotionAsync(int productId, int promotionId, bool isActive);

    }
}