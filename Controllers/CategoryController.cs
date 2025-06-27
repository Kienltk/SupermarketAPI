using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet("")]
        public async Task<ActionResult<List<CategoriesDto>>> GetBrand()
        {
            try
            {
                var categories = await _categoryService.GetCategories();
                if (categories == null || !categories.Any())
                {
                    var errorResponse = new ResponseObject<String>
                    {
                        Code = 404,
                        Message = "Not found",
                        Data = null
                    };
                    return NotFound(errorResponse);
                }
                var response = new ResponseObject<List<CategoriesDto>>
                {
                    Code = 200,
                    Message = "Get Categories successful",
                    Data = categories
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
