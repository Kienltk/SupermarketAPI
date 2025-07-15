using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Services;
using System.Threading.Tasks;

namespace SupermarketAPI.Controllers
{
    [ApiController]
    [Route("api/dashboard")]
    [Authorize(Roles = "ADMIN")]
    public class DashboardController : ControllerBase
    {
        private readonly IDashboardService _dashboardService;

        public DashboardController(IDashboardService dashboardService)
        {
            _dashboardService = dashboardService;
        }

        [HttpGet]
        public async Task<ActionResult<ResponseObject<DashboardDto>>> GetDashboardData()
        {
            try
            {
                var dashboardData = await _dashboardService.GetDashboardData();
                var response = new ResponseObject<DashboardDto>
                {
                    Code = 200,
                    Message = "Get Dashboard Data successful",
                    Data = dashboardData
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
