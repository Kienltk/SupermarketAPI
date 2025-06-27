using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;
using SupermarketAPI.Services.Impl;
using System.Security.Claims;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/brands")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<BrandDto>>> GetBrand()
        {
            try
            {
                var brands = await _brandService.GetBrands();
                if (brands == null || !brands.Any())
                {
                    var errorResponse = new ResponseObject<String>
                    {
                        Code = 404,
                        Message = "Not found",
                        Data = null
                    };
                    return NotFound(errorResponse);
                }
                var response = new ResponseObject<List<BrandDto>>
                {
                    Code = 200,
                    Message = "Get Brands successful",
                    Data = brands
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
