using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories
{
    public interface IBillRepository
    {
        Task CreateBill(Bill bill);
        Task UpdateBill (Bill bill);
        Task CreateBillDetail(BillDetail billDetail);
        Task<Bill> GetBillByOrderId(int orderId);
    }
}
