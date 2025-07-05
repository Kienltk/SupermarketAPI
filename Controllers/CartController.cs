using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Services.Impl;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/carts")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;

        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }


        [Authorize]
        [HttpGet("")]
        public async Task<ActionResult<ResponseObject<List<CartItemDTO>>>> GetCartItems()
        {
            if (!User?.Identity?.IsAuthenticated == true)
            {
                var errorResponse = new ResponseObject<String>
                {
                    Code = 401,
                    Message = "Unauthorize",
                    Data = null
                };
                return Unauthorized(errorResponse);
            }

            try
            {
                List<CartItemDTO> products;
                int customerId;
                if (int.TryParse(User?.FindFirst("id")?.Value, out int id))
                {
                    customerId = id;
                }
                else
                {
                    var errorResponse = new ResponseObject<String>
                    {
                        Code = 404,
                        Message = "No user found",
                        Data = null
                    };
                    return NotFound(errorResponse);
                }

                products = await _cartService.GetCartItemsByUserIdAsync(customerId);

                var response = new ResponseObject<List<CartItemDTO>>
                {
                    Code = 200,
                    Message = "Get Products successful",
                    Data = products
                };
                return Ok(response);


            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<String>
                {
                    Code = 500,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<ActionResult<ResponseObject<string>>> AddCartItems([FromBody] List<CartItemDTO> cartItems)
        {
            if (!User?.Identity?.IsAuthenticated == true)
            {
                var errorResponse = new ResponseObject<String>
                {
                    Code = 401,
                    Message = "Unauthorize",
                    Data = null
                };
                return Unauthorized(errorResponse);
            }

            if (!int.TryParse(User?.FindFirst("id")?.Value, out int customerId))
            {
                var errorResponse = new ResponseObject<String>
                {
                    Code = 404,
                    Message = "No user found",
                    Data = null
                };
                return NotFound(errorResponse);
            }

            try
            {
                var success = await _cartService.AddCartItemsAsync(customerId, cartItems);
                if (!success)
                {
                    return BadRequest(new ResponseObject<string>
                    {
                        Code = 400,
                        Message = "Failed to add cart items. Invalid product or quantity.",
                        Data = null
                    });
                }

                return Ok(new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Cart items added successfully",
                    Data = true
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseObject<string>
                {
                    Code = 500,
                    Message = $"Internal server error: {ex.Message}",
                    Data = null
                });
            }
        }
    }
}
