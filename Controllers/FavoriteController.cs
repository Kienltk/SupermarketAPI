using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using SupermarketAPI.Services.Impl;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/favorites")]
    public class FavoriteController : Controller
    {
        private readonly IFavoriteService _favoriteService;

        public FavoriteController(IFavoriteService favoriteService)
        {
            _favoriteService = favoriteService;
        }

        [HttpGet("")]
        public async Task<ActionResult<ResponseObject<List<ProductDto>>>> GetFavoriteProducts()
        {
            int customerId;
            if (User?.Identity?.IsAuthenticated == false)
            {
                return Unauthorized(new ResponseObject<string>
                {
                    Code = 401,
                    Message = "Unauthorized",
                    Data = null
                });
            }

            if (int.TryParse(User?.FindFirst("id")?.Value, out var id))
            {
                customerId = id;
            } else
            {
                return NotFound(new ResponseObject<string>
                {
                    Code = 404,
                    Message = "No user found",
                    Data = null
                });
            }

            try
            {
                Console.WriteLine("Customer id: " + customerId);
                var products = await _favoriteService.GetFavoritesProductByUserId(customerId);
                if (products == null || !products.Any())
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "No products found",
                        Data = null
                    });
                }
                var response = new ResponseObject<List<ProductDto>>
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
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpPost("{productId}")]
        public async Task<IActionResult> AddFavorite(int productId)
        {
            int customerId;
            if (User?.Identity?.IsAuthenticated == false)
            {
                return Unauthorized(new ResponseObject<string>
                {
                    Code = 401,
                    Message = "Unauthorized",
                    Data = null
                });
            }

            if (int.TryParse(User?.FindFirst("id")?.Value, out var id))
            {
                customerId = id;
            }
            else
            {
                return NotFound(new ResponseObject<string>
                {
                    Code = 404,
                    Message = "No user found",
                    Data = null
                });
            }

            try
            {
                Console.WriteLine("CustomerId: " + customerId + ", productId: " + productId);
                var success = await _favoriteService.AddFavorite(customerId, productId);
                if (!success)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "No products found",
                        Data = null
                    });
                }
                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Favorite added successfully",
                    Data = success
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<String>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteFavorite(int productId)
        {
            int customerId;
            if (User?.Identity?.IsAuthenticated == false)
            {
                return Unauthorized(new ResponseObject<string>
                {
                    Code = 401,
                    Message = "Unauthorized",
                    Data = null
                });
            }

            if (int.TryParse(User?.FindFirst("id")?.Value, out var id))
            {
                customerId = id;
            }
            else
            {
                return NotFound(new ResponseObject<string>
                {
                    Code = 404,
                    Message = "No user found",
                    Data = null
                });
            }

            try
            {
                var success = await _favoriteService.DeleteFavorite(customerId, productId);
                if (!success)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "No products found",
                        Data = null
                    });
                }
                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Favorite deleted successfully",
                    Data = success
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<String>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }
    }
}
