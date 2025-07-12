using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IProductService _productService;

        public ProductController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet("home")]
        public async Task<ActionResult<ResponseObject<HomeDto>>> Home()
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                var products = await _productService.Home(customerId);
                var response = new ResponseObject<HomeDto>
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
        [HttpPost]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            var result = await _productService.CreateProductAsync(product);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(int id, [FromBody] Product product)
        {
            var result = await _productService.UpdateProductAsync(id, product);
            if (result == null) return NotFound();
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var success = await _productService.DeleteProductAsync(id);
            if (!success) return NotFound();
            return NoContent();
        }
        [HttpGet("{slug}")]
        public async Task<ActionResult<ProductDetailDto>> GetProductDetails(string slug)
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                var productDetails = await _productService.GetProductDetailsAsync(slug, customerId);
                if (productDetails == null)
                {
                    var errorResponse = new ResponseObject<String>
                    {
                        Code = 404,
                        Message = "Not found",
                        Data = null
                    };
                    return NotFound(errorResponse);
                }
                var response = new ResponseObject<ProductDetailDto>
                {
                    Code = 200,
                    Message = "Get Produt detail successful",
                    Data = productDetails
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

        [HttpGet("category/{category}")]
        public async Task<ActionResult<ResponseObject<Dictionary<string, List<ProductDto>>>>> GetProductsByCategory(string category)
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                Console.WriteLine("Category:" + category);
                var products = await _productService.GetProductsByCategoryAsync(customerId, category);
                if (products == null || !products.Any())
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "No products found",
                        Data = null
                    });
                }
                var response = new ResponseObject<Dictionary<string, List<ProductDto>>>
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

        [HttpGet("brand/{brand}")]
        public async Task<ActionResult<ResponseObject<List<ProductDto>>>> GetProductsByBrand(string brand)
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                var products = await _productService.GetProductsByBrand(customerId, brand);
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

        [HttpGet("")]
        public async Task<ActionResult<List<ProductDto>>> GetProducts([FromQuery] string? searchName
                                                                     , [FromQuery] decimal? minPrice, [FromQuery] decimal? maxPrice)
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                List<ProductDto> products;
                if (string.IsNullOrEmpty(searchName))
                {
                    if (minPrice.HasValue || maxPrice.HasValue)
                    {
                        decimal min = minPrice ?? 0;
                        decimal max = maxPrice ?? 999999999;
                        products = await _productService.GetProductsByPrice(customerId, min, max);
                    }
                    else
                    {
                        products = await _productService.GetProducts(customerId);
                    }
                }
                else
                {
                    if (minPrice.HasValue || maxPrice.HasValue)
                    {
                        decimal min = minPrice ?? 0;
                        decimal max = maxPrice ?? 999999999;
                        products = await _productService.GetProductsByProductNameAndPrice(customerId, searchName, min, max);
                    }
                    else
                    {
                        products = await _productService.GetProductsByProductName(customerId, searchName);
                    }
                }

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
                    Message = "Get Produts successful",
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

        [HttpGet("filter")]
        public async Task<ActionResult<List<ProductDto>>> GetProductsByBrandAndCategory(
             [FromQuery] string? category
            ,[FromQuery] string? brand
            ,[FromQuery] int? ratingScore)
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                if (string.IsNullOrEmpty(brand)) brand = null;
                if (string.IsNullOrEmpty(category))  category = null;
                if (!ratingScore.HasValue) ratingScore = null;

                List<ProductDto> products = await _productService.GetProductsByBrandAndCategoryAndRating(customerId, category, brand, ratingScore);

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
                    Message = "Get Produts successful",
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

        [HttpGet("rating")]
        public async Task<ActionResult<ResponseObject<List<ProductDto>>>> GetProductsByRatingScore([FromQuery] int ratingScore)
        {
            int? customerId = null;
            if (User?.Identity?.IsAuthenticated == true)
            {
                if (int.TryParse(User.FindFirst("id")?.Value, out var id))
                {
                    customerId = id;
                }
            }

            try
            {
                var products = await _productService.GetProductbyRatingScore(customerId, ratingScore);
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
    }
}