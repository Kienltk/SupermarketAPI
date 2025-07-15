using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SupermarketAPI.Services.Impl
{
    public class DashboardService : IDashboardService
    {
        private readonly ProductService _productService;
        private readonly IOrderRepository _orderRepository;
        private readonly ICategoryRepository _categoryRepository;



        public DashboardService(ProductService productService, IOrderRepository orderRepository, ICategoryRepository categoryRepository)
        {
            _productService = productService;
            _orderRepository = orderRepository;
            _categoryRepository = categoryRepository;
        }

        public async Task<DashboardDto> GetDashboardData()
        {
            var dashboard = new DashboardDto
            {
                TotalIncome = await _orderRepository.GetTotalIncome(),
                TotalOrder = await _orderRepository.GetTotalOrder(),
                TopProductByParentCategory = await GetTopProductsByParentCategory(),
                revenueChart = await GetRevenueChart()
            };

            return dashboard;
        }

        private async Task<Dictionary<string, List<ProductDto>>> GetTopProductsByParentCategory()
        {
            var parentCategories = await _categoryRepository.GetCategoriesByParentIdAsync(null);
            var result = new Dictionary<string, List<ProductDto>>();

            foreach (var parent in parentCategories)
            {
                var topProducts = await _orderRepository.GetTopProductsByCategoryIdAsync(parent.CategoryId, 3);
                result.Add(parent.CategoryName, topProducts.Select(p => _productService.MapToProductDto(p.Product, null)).ToList());
            }

            return result;
        }

        private async Task<List<RevenueDto>> GetRevenueChart()
        {
            var revenueData = await _orderRepository.GetRevenueByDateAsync();
            return revenueData.Select(r => new RevenueDto
            {
                date = r.Date.ToString("dd/MM/yyyy"),
                total = r.Total
            }).ToList();
        }
    }
}