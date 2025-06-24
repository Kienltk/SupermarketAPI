using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
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
        public async Task<IActionResult> Home()
        {
            var customerId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            try
            {
                var products = await _productService.Home(customerId);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }


        [HttpGet("category/{slug}")]
        public async Task<IActionResult> GetProductsByCategory(string slug)
        {
            var customerId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            try
            {
                var products = await _productService.GetProductsByCategoryAsync(customerId, slug);
                return Ok(products);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{slug}")]
        public async Task<IActionResult> GetProductDetails(string slug)
        {
            var customerId = User.Identity.IsAuthenticated
                ? int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)?.Value)
                : (int?)null;

            try
            {
                var productDetails = await _productService.GetProductDetailsAsync(slug, customerId);
                if (productDetails == null)
                    return NotFound("Product not found");
                return Ok(productDetails);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}