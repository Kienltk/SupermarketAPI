using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/ratings")]
    public class RatingController : ControllerBase
    {
        private readonly IRatingService _ratingService;

        public RatingController(IRatingService ratingService)
        {
            _ratingService = ratingService;
        }

        [HttpGet("product/{productSlug}")]
        public async Task<ActionResult<ResponseObject<List<RatingDto>>>> GetRatingsByProduct(string productSlug)
        {
            try
            {
                var ratings = await _ratingService.GetRatingsByProductSlugAsync(productSlug);
                return Ok(new ResponseObject<List<RatingDto>>
                {
                    Code = 200,
                    Message = "Get ratings successful",
                    Data = ratings
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

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateRating([FromBody] RatingCreateDto dto)
        {
            try
            {
                var customerId = int.Parse(User.FindFirst("id").Value);
                await _ratingService.CreateRatingAsync(customerId, dto);

                return Ok(new ResponseObject<string>
                {
                    Code = 201,
                    Message = "Rating created",
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

        [HttpPut("{ratingId}")]
        [Authorize]
        public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] RatingUpdateDto dto)
        {
            try
            {
                var customerId = int.Parse(User.FindFirst("id").Value);
                bool isAdmin = User.IsInRole("admin");

                await _ratingService.UpdateRatingAsync(ratingId, customerId, dto, isAdmin);

                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Rating updated",
                    Data = null
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<string>
                {
                    Code = 404,
                    Message = ex.Message,
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

        [HttpDelete("{ratingId}")]
        [Authorize]
        public async Task<IActionResult> DeleteRating(int ratingId)
        {
            try
            {
                var customerId = int.Parse(User.FindFirst("id").Value);
                bool isAdmin = User.IsInRole("admin");

                await _ratingService.DeleteRatingAsync(ratingId, customerId, isAdmin);

                return Ok(new ResponseObject<string>
                {
                    Code = 200,
                    Message = "Rating deleted",
                    Data = null
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Forbid(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseObject<string>
                {
                    Code = 404,
                    Message = ex.Message,
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
