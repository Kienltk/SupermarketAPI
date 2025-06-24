using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Services;
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
            var customerId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

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


        [HttpGet("category/{slug}")]
        public async Task<ActionResult<ResponseObject<Dictionary<string, List<ProductDto>>>>> GetProductsByCategory(string slug)
        {
            var customerId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            try
            {
                var products = await _productService.GetProductsByCategoryAsync(customerId, slug);
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

        [HttpGet("{slug}")]
        public async Task<ActionResult<ProductDetailDto>> GetProductDetails(string slug)
        {
            var customerId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

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
                    Message = "Get Produt detal successful",
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
    }
}