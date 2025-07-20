using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketAPI.Repositories.Impl
{
    public class PromotionRepository : IPromotionRepository
    {
        private readonly SupermarketContext _context;

        public PromotionRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task CreatePromotionAsync(Promotion promotion)
        {
            await _context.Promotions.AddAsync(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Promotion>> GetAllPromotionsAsync()
        {
            return await _context.Promotions
                .Include(p => p.GiftProduct)
                .ToListAsync();
        }

        public async Task<Promotion> GetPromotionByIdAsync(int promotionId)
        {
            return await _context.Promotions
                .Include(p => p.GiftProduct)
                .FirstOrDefaultAsync(p => p.PromotionId == promotionId);
        }

        public async Task UpdatePromotionAsync(Promotion promotion)
        {
            _context.Promotions.Update(promotion);
            await _context.SaveChangesAsync();
        }

        public async Task AddDiscountAsync(Discount discount)
        {
            await _context.Discounts.AddAsync(discount);
            await _context.SaveChangesAsync();
        }

        public async Task<Discount> GetDiscountAsync(int productId, int promotionId)
        {
            return await _context.Discounts
                .FirstOrDefaultAsync(d => d.ProductId == productId && d.PromotionId == promotionId);
        }

        public async Task UpdateDiscountAsync(Discount discount)
        {
            _context.Discounts.Update(discount);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Discount>> GetPromotionByProductIdAsync(int productId)
        {
            return await _context.Discounts.Where(d => d.ProductId == productId).ToListAsync();
        }

        public async Task<List<Product>> GetProductsByPromotionIdAsync(int promotionId)
        {
            return await _context.Discounts
                .Where(d => d.PromotionId == promotionId && d.IsActive == true)
                .Include(d => d.Product)
                .Select(d => d.Product)
                .ToListAsync();
        }
    }
}