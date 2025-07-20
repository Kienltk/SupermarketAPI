using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;
using System.Threading.Tasks;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/promotions")]
    [Authorize(Roles = "ADMIN")]
    public class PromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;

        public PromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        [HttpPost]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseObject<PromotionDto>>> CreatePromotion([FromBody] PromotionDto promotionRequest)
        {
            try
            {
                var promotion = await _promotionService.CreatePromotionAsync(promotionRequest);
                var response = new ResponseObject<PromotionDto>
                {
                    Code = 201,
                    Message = "Promotion created successfully",
                    Data = promotion
                };
                return CreatedAtAction(nameof(GetPromotionById), new { promotionId = promotion.PromotionId }, response);
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

        [HttpGet]
        public async Task<ActionResult<ResponseObject<List<PromotionDto>>>> GetAllPromotions()
        {
            try
            {
                var promotions = await _promotionService.GetAllPromotionsAsync();
                var response = new ResponseObject<List<PromotionDto>>
                {
                    Code = 200,
                    Message = "Promotions retrieved successfully",
                    Data = promotions
                };
                return Ok(response);
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

        [HttpGet("{promotionId}")]
        public async Task<ActionResult<ResponseObject<PromotionDto>>> GetPromotionById(int promotionId)
        {
            try
            {
                var promotion = await _promotionService.GetPromotionByIdAsync(promotionId);
                var response = new ResponseObject<PromotionDto>
                {
                    Code = 200,
                    Message = "Promotion retrieved successfully",
                    Data = promotion
                };
                return Ok(response);
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

        [HttpPut("{promotionId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseObject<bool>>> UpdatePromotion(int promotionId, [FromBody] bool isActive)
        {
            try
            {
                var result = await _promotionService.UpdatePromotionAsync(promotionId, isActive);
                if (!result)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "Promotion not found",
                        Data = null
                    });
                }

                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Promotion updated successfully",
                    Data = true
                };
                return Ok(response);
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

        [HttpPost("{promotionId}/products/{productId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseObject<bool>>> AddProductToPromotion(int promotionId, int productId)
        {
            try
            {
                var result = await _promotionService.AddProductToPromotionAsync(productId, promotionId);
                if (!result)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "Promotion or Product not found",
                        Data = null
                    });
                }

                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Product added to promotion successfully",
                    Data = true
                };
                return Ok(response);
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

        [HttpPut("{promotionId}/products/{productId}")]
        [Authorize(Roles = "ADMIN")]
        public async Task<ActionResult<ResponseObject<bool>>> UpdateProductPromotion(int promotionId, int productId, [FromBody] bool isActive)
        {
            try
            {
                var result = await _promotionService.UpdateProductPromotionAsync(productId, promotionId, isActive);
                if (!result)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "Product-Promotion relationship not found",
                        Data = null
                    });
                }

                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Product-Promotion relationship updated successfully",
                    Data = true
                };
                return Ok(response);
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
