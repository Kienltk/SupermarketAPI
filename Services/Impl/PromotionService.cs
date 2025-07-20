using Microsoft.EntityFrameworkCore;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketAPI.Services.Impl
{
    public class PromotionService : IPromotionService
    {
        private readonly IPromotionRepository _promotionRepository;
        private readonly IProductRepository _productRepository;
        private readonly ProductService _productService;

        public PromotionService(IPromotionRepository promotionRepository, ProductService productService, IProductRepository productRepository)
        {
            _promotionRepository = promotionRepository;
            _productService = productService;
            _productRepository = productRepository;
        }

        public async Task<PromotionDto> CreatePromotionAsync(PromotionDto promotionRequest)
        {
            var promotion = new Promotion
            {
                PromotionType = promotionRequest.PromotionType,
                Description = promotionRequest.Description,
                StartDate = promotionRequest.StartDate ?? DateTime.Now,
                EndDate = promotionRequest.EndDate ?? DateTime.Now,
                DiscountPercent = promotionRequest.DiscountPercent ?? null,
                DiscountAmount = promotionRequest.DiscountAmount ?? null,
                GiftProductId = promotionRequest.GiftProductId ?? null,
                MinOrderValue = promotionRequest.MinOrderValue ?? null,
                MinOrderQuantity = promotionRequest.MinOrderQuantity ?? null,
                IsActive = false
            };

            await _promotionRepository.CreatePromotionAsync(promotion);

            return await MapToPromotionDto(promotion);
        }

        public async Task<List<PromotionDto>> GetAllPromotionsAsync()
        {
            var promotions = await _promotionRepository.GetAllPromotionsAsync();
            var promotionDtos = new List<PromotionDto>();

            foreach (var promotion in promotions)
            {
                promotionDtos.Add(await MapToPromotionDto(promotion));
            }

            return promotionDtos;
        }

        public async Task<PromotionDto> GetPromotionByIdAsync(int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                throw new Exception("Promotion not found");
            }

            return await MapToPromotionDto(promotion);
        }

        public async Task<bool> UpdatePromotionAsync(int promotionId, bool isActive)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                return false;
            }

            promotion.IsActive = isActive;
            await _promotionRepository.UpdatePromotionAsync(promotion);
            return true;
        }

        public async Task<bool> AddProductToPromotionAsync(int productId, int promotionId)
        {
            var promotion = await _promotionRepository.GetPromotionByIdAsync(promotionId);
            if (promotion == null)
            {
                return false;
            }

            var product = await _productRepository.GetProductByIdAsync(productId);
            if (product == null)
            {
                return false;
            }

            var oldPromotions = await _promotionRepository.GetPromotionByProductIdAsync(productId);

            // Deactivate all existing promotions
            if (oldPromotions?.Any() == true)
            {
                foreach (var oldPromotion in oldPromotions)
                {
                    if (oldPromotion.IsActive)
                    {
                        await UpdateProductPromotionAsync(productId, oldPromotion.PromotionId, false);
                    }
                }
            }

            // Check if promotionId already exists
            var existingPromotion = oldPromotions?.FirstOrDefault(p => p.PromotionId == promotionId);

            if (existingPromotion == null)
            {
                var discount = new Discount
                {
                    ProductId = productId,
                    PromotionId = promotionId,
                    IsActive = true
                };
                await _promotionRepository.AddDiscountAsync(discount);
            }
            else if (!existingPromotion.IsActive)
            {
                await UpdateProductPromotionAsync(productId, promotionId, true);
            }

            return true;
        }

        public async Task<bool> UpdateProductPromotionAsync(int productId, int promotionId, bool isActive)
        {
            var discount = await _promotionRepository.GetDiscountAsync(productId, promotionId);
            if (discount == null)
            {
                return false;
            }
            Console.WriteLine("Is Active: " + isActive);

            discount.IsActive = isActive;
            await _promotionRepository.UpdateDiscountAsync(discount);
            return true;
        }

        private async Task<PromotionDto> MapToPromotionDto(Promotion promotion)
        {
            var products = await _promotionRepository.GetProductsByPromotionIdAsync(promotion.PromotionId);
            return new PromotionDto
            {
                PromotionId = promotion.PromotionId,
                PromotionType = promotion.PromotionType,
                Description = promotion.Description,
                StartDate = promotion.StartDate,
                EndDate = promotion.EndDate,
                DiscountPercent = promotion.DiscountPercent,
                DiscountAmount = promotion.DiscountAmount,
                GiftProductId = promotion.GiftProductId,
                GiftProductName = promotion.GiftProduct?.ProductName,
                GiftProductImg = promotion.GiftProduct?.ImageUrl,
                GiftProductSlug = promotion.GiftProduct?.Slug,
                MinOrderValue = promotion.MinOrderValue,
                MinOrderQuantity = promotion.MinOrderQuantity,
                IsActive = promotion.IsActive,
                Products = products.Select(p => _productService.MapToProductDto(p, null)).ToList()
            };
        }
    }
}
