namespace SupermarketAPI.DTOs.Response
{
    public class DashboardDto
    {
        public decimal TotalIncome { get; set; }
        public int TotalOrder { get; set; }
        public Dictionary<string, List<ProductDto>> TopProductByParentCategory { get; set; }
        public List<RevenueDto> revenueChart { get; set; }


    }
}
