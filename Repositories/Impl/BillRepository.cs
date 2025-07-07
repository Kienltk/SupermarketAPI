using Microsoft.EntityFrameworkCore;
using SupermarketAPI.Data;
using SupermarketAPI.Models;

namespace SupermarketAPI.Repositories.Impl
{
    public class BillRepository : IBillRepository
    {
        private readonly SupermarketContext _context;

        public BillRepository(SupermarketContext context)
        {
            _context = context;
        }

        public async Task CreateBill(Bill bill)
        {
            _context.Bills.Add(bill);
            await _context.SaveChangesAsync();
        }

        public async Task CreateBillDetail(BillDetail billDetail)
        {
            _context.BillDetails.Add(billDetail);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateBill(Bill bill)
        {
            _context.Bills.Update(bill);
            await _context.SaveChangesAsync();
        }

        public  async Task<Bill> GetBillByOrderId(int orderId)
        {
            return await _context.Bills
                .Include(b => b.BillDetails)
                .FirstAsync(b => b.OrderId == orderId);
        }
    }
}
