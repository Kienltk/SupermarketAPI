// BrandController.cs
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Authorize(Roles = "ADMIN")]
    [Route("api/brands")]
    public class BrandController : Controller
    {
        private readonly IBrandService _brandService;

        public BrandController(IBrandService brandService)
        {
            _brandService = brandService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<BrandDto>>> GetBrands()
        {
            try
            {
                var brands = await _brandService.GetBrands();
                if (brands == null || !brands.Any())
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "Not found",
                        Data = null
                    });
                }

                return Ok(new ResponseObject<List<BrandDto>>
                {
                    Code = 200,
                    Message = "Get Brands successful",
                    Data = brands
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetBrandById(int id)
        {
            try
            {
                var brand = await _brandService.GetBrandByIdAsync(id);
                return Ok(new ResponseObject<BrandDto>
                {
                    Code = 200,
                    Message = "Get brand successful",
                    Data = brand
                });
            }
            catch (Exception ex)
            {
                return NotFound(new ResponseObject<string>
                {
                    Code = 404,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPost("")]
        public async Task<IActionResult> CreateBrand([FromBody] BrandDto brandDto)
        {
            try
            {
                var created = await _brandService.CreateBrandAsync(brandDto);
                return Ok(new ResponseObject<BrandDto>
                {
                    Code = 201,
                    Message = "Brand created successfully",
                    Data = created
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBrand(int id, [FromBody] BrandDto brandDto)
        {
            try
            {
                var updated = await _brandService.UpdateBrandAsync(id, brandDto);
                return Ok(new ResponseObject<BrandDto>
                {
                    Code = 200,
                    Message = "Brand updated successfully",
                    Data = updated
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBrand(int id)
        {
            try
            {
                await _brandService.DeleteBrandAsync(id);
                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Brand deleted successfully",
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
        }
    }
}
