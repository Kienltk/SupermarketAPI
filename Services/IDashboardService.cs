using SupermarketAPI.DTOs.Response;

namespace SupermarketAPI.Services
{
    public interface IDashboardService
    {
        Task<DashboardDto> GetDashboardData();
    }
}
