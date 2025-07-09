using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize]
        [HttpPost()]
        public async Task<ActionResult<ResponseObject<bool>>> CreateOrder([FromBody] OrderRequestDto orderRequestDto)
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
                var result = await _orderService.CreateOrder(customerId, orderRequestDto);
                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Order created successfully",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [Authorize]
        [HttpGet()]
        public async Task<ActionResult<ResponseObject<List<OrderDto>>>> GetOrdersByCustomerId()
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
                var orders = await _orderService.GetOrdersByCustomerId(customerId);
                var response = new ResponseObject<List<OrderDto>>
                {
                    Code = 200,
                    Message = "Get Orders successful",
                    Data = orders
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [Authorize]
        [HttpPut("bill")]
        public async Task<ActionResult<ResponseObject<bool>>> UpdateBill([FromBody] BillUpdateDto billUpdateDto)
        {
            if (User?.Identity?.IsAuthenticated == false)
            {
                return Unauthorized(new ResponseObject<string>
                {
                    Code = 401,
                    Message = "Unauthorized",
                    Data = null
                });
            }
            try
            {
                var result = await _orderService.UpdateBill(billUpdateDto);
                if (!result)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "Bill not found",
                        Data = null
                    });
                }
                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Bill updated successfully",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<string>
                {
                    Code = 400,
                    Message = ex.Message,
                    Data = null
                };
                return BadRequest(errorResponse);
            }
        }

        [Authorize]
        [HttpPut("")]
        public async Task<ActionResult<ResponseObject<bool>>> UpdateOrder([FromBody] OrderUpdateDto orderUpdateDto)
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
                var result = await _orderService.UpdateOrder(customerId, orderUpdateDto);
                if (!result)
                {
                    return NotFound(new ResponseObject<string>
                    {
                        Code = 404,
                        Message = "Order not found",
                        Data = null
                    });
                }
                var response = new ResponseObject<bool>
                {
                    Code = 200,
                    Message = "Order updated successfully",
                    Data = result
                };
                return Ok(response);
            }
            catch (Exception ex)
            {
                var errorResponse = new ResponseObject<string>
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