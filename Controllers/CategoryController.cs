using Microsoft.AspNetCore.Authorization;
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

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
        {
            try
            {
                var category = await _categoryService.GetCategoryByIdAsync(id);
                return Ok(new ResponseObject<CategoryDto>
                {
                    Code = 200,
                    Message = "Get category successful",
                    Data = category
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
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            try
            {
                await _categoryService.DeleteCategoryAsync(id);
                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Category deleted successfully",
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
        [HttpGet("")]
        public async Task<ActionResult<List<CategoriesDto>>> GetCategories()
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
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, [FromBody] CategoryDto categoryDto)
        {
            try
            {
                var updated = await _categoryService.UpdateCategoryAsync(id, categoryDto);
                return Ok(new ResponseObject<CategoryDto>
                {
                    Code = 200,
                    Message = "Category updated successfully",
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
        [HttpPost("")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto newCategory)
        {
            try
            {
                var created = await _categoryService.CreateCategoryAsync(newCategory);
                return Ok(new ResponseObject<CategoryDto>
                {
                    Code = 201,
                    Message = "Category created successfully",
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

    }
}
