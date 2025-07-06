using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;

namespace SupermarketAPI.Services.Impl
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;

        public CartService(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }

        public async Task<List<CartItemDto>> GetCartItemsByUserIdAsync(int customerId)
        {
            var cartItems = await _cartRepository.GetCartItemsByUserIdAsync(customerId);
            var cartItemDtos = new List<CartItemDto>();

            foreach (var cartItem in cartItems)
            {
                if (cartItem?.Product != null)
                {
                    var cartItemDto = MapToCartItemDto(cartItem);
                    cartItemDtos.Add(cartItemDto);
                }
            }

            return cartItemDtos;
        }

        public async Task<bool> AddCartItemsAsync(int customerId, List<CartItemDto> cartItems)
        {
            var cart = await _cartRepository.GetCartByCustomerIdAsync(customerId);
            if (cart == null)
            {
                cart = new Cart { CustomerId = customerId };
                await _cartRepository.CreateCartAsync(cart);
            }

            var existingCartItems = await _cartRepository.GetCartItemsByUserIdAsync(customerId);
            if (existingCartItems.Any())
            {
                await _cartRepository.DeleteCartItemsAsync(existingCartItems);
            }

            var newCartItems = cartItems.Select(dto => new CartItem
            {
                CartId = cart.CartId,
                ProductId = dto.ProductId,
                Quantity = dto.Quantity
            }).ToList();

            await _cartRepository.AddCartItemsAsync(newCartItems);
            return true;
        }

        private CartItemDto MapToCartItemDto(CartItem cartItem)
        {
            if (cartItem == null || cartItem.Product == null)
            {
                throw new ArgumentNullException(nameof(cartItem), "CartItem or Product is null");
            }

            var promotion = cartItem.Product.Discounts?.FirstOrDefault()?.Promotion;

            return new CartItemDto
            {
                ProductId = cartItem.ProductId,
                ProductName = cartItem.Product.ProductName,
                Price = cartItem.Product.Price,
                Slug = cartItem.Product.Slug,
                Status = cartItem.Product.Status,
                ImageUrl = cartItem.Product.ImageUrl,
                Stock = cartItem.Product.Quantity,
                Quantity = cartItem.Quantity,
                PromotionId = promotion?.PromotionId,
                PromotionType = promotion?.PromotionType,
                PromotionDescription = promotion?.Description,
                DiscountPercent = promotion?.DiscountPercent,
                DiscountAmount = promotion?.DiscountAmount,
                GiftProductId = promotion?.GiftProductId,
                GiftProductName = promotion?.GiftProduct?.ProductName,
                GiftProductSlug = promotion?.GiftProduct?.Slug,
                GiftProductImg = promotion?.GiftProduct?.ImageUrl,
                MinOrderValue = promotion?.MinOrderValue,
                MinOrderQuantity = promotion?.MinOrderQuantity,
                StartDate = promotion?.StartDate ?? DateTime.MinValue,
                EndDate = promotion?.EndDate ?? DateTime.MinValue
            };
        }
    }
}
