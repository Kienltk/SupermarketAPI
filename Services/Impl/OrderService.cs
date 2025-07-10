using Microsoft.Identity.Client;
using SupermarketAPI.DTOs.Request;
using SupermarketAPI.DTOs.Response;
using SupermarketAPI.Models;
using SupermarketAPI.Repositories;
using SupermarketAPI.Repositories.Impl;

namespace SupermarketAPI.Services.Impl
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IBillRepository _billRepository;
        private decimal TAX_PERCENT = 8;

        public OrderService(IOrderRepository orderRepository, IBillRepository billRepository)
        {
            _orderRepository = orderRepository;
            _billRepository = billRepository;
        }

        public async Task<bool> CreateOrder(int customerId, OrderRequestDto orderRequestDto)
        {
            var order = new Order
            {
                CustomerId = customerId,
                DateOfPurchase = DateTime.Now,
                Status = "PENDING",
                //Amount = orderRequestDto.Items.Sum(item =>
                //{
                //    var itemAmount = Math.Round((item.Price - (item.Price * (item.DiscountPercent ?? 0) / 100)) * item.Quantity - (item.DiscountAmount ?? 0), 2);
                //    Console.WriteLine($"Item: Price={item.Price}, Discount={item.DiscountPercent}%, Quantity={item.Quantity}, DiscountAmount={item.DiscountAmount}, Total={itemAmount}");
                //    return itemAmount;
                //})
                Amount = orderRequestDto.TotalAmount,
            };
            await _orderRepository.CreateOrder(order);

            foreach (var item in orderRequestDto.Items)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = order.OrderId,
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    UnitPrice = item.Price,
                    PromotionId = item.PromotionId
                };
                await _orderRepository.CreateOrderDetail(orderDetail);
            }

            decimal taxAmount = Math.Round(order.Amount * (TAX_PERCENT/100), 2); 
            decimal shippingFee = 1; 

            var bill = new Bill
            {
                OrderId = order.OrderId,
                BillAmount = order.Amount + taxAmount + shippingFee,
                PaymentMethod = orderRequestDto.PaymentMethod,
                PaymentStatus = orderRequestDto.IsPay ? "COMPLETED" : "PENDING"
            };
            Console.WriteLine($"Order Amount={order.Amount}, Tax Amount={taxAmount}%, Shipping Fee={shippingFee}");
            await _billRepository.CreateBill(bill);

            var taxDetail = new BillDetail
            {
                BillId = bill.BillId,
                ItemType = "TAX",
                Description = "VAT 8%",
                Amount = taxAmount
            };
            await _billRepository.CreateBillDetail(taxDetail);

            var feeDetail = new BillDetail
            {
                BillId = bill.BillId,
                ItemType = "FEE",
                Description = "Shipping fee",
                Amount = shippingFee
            };
            await _billRepository.CreateBillDetail(feeDetail);

            return true;
        }

        public async Task<List<OrderDto>> GetOrdersByCustomerId(int customerId)
        {
            var orders = await _orderRepository.GetOrderByUserId(customerId);
            var orderDtos = new List<OrderDto>();

            foreach (var order in orders)
            {
                var bill = await _billRepository.GetBillByOrderId(order.OrderId);
                var orderDto = new OrderDto
                {
                    BillId = bill.BillId,
                    OrderId = order.OrderId,
                    OrderStatus = order.Status,
                    DateOfPurchase = order.DateOfPurchase,
                    OrderAmount = order.Amount,
                    BillAmount = bill.BillAmount,
                    PaymentMethod = bill.PaymentMethod,
                    PaymentStatus = bill.PaymentStatus,
                    OrderItems = order.OrderDetails.Select(od => new OrderItemDto
                    {
                        ProductId = od.ProductId,
                        ProductName = od.Product.ProductName,
                        Price = od.UnitPrice,
                        Slug = od.Product.Slug,
                        ImageUrl = od.Product.ImageUrl,
                        Quantity = od.Quantity,
                        PromotionId = od.PromotionId ?? null,
                        PromotionType = od.Promotion.PromotionType ?? null,
                        PromotionDescription = od.Promotion.Description ?? null,
                        DiscountPercent = od.Promotion.DiscountPercent ?? null,
                        DiscountAmount = od.Promotion.DiscountAmount ?? null,
                        GiftProductId = od.Promotion.GiftProductId ?? null,
                        GiftProductName = od.Promotion.GiftProduct?.ProductName ?? null,
                        GiftProductSlug = od.Promotion.GiftProduct?.Slug ?? null,
                        GiftProductImg = od.Promotion.GiftProduct?.ImageUrl ?? null,
                        MinOrderValue = od.Promotion.MinOrderValue ?? null,
                        MinOrderQuantity = od.Promotion.MinOrderQuantity ?? null,
                    }).ToList() ?? new List<OrderItemDto>(),
                    BillDetails = bill.BillDetails.Select(bd => new BillDetailDto
                    {
                        ItemType = bd.ItemType,
                        Amount = bd.Amount,
                        Description = bd.Description,
                    }).ToList()
                };
                Console.WriteLine("OrderDto" + orderDto.ToString);
                orderDtos.Add(orderDto);
            }

            return orderDtos;
        }

        public async Task<bool> UpdateBill(BillUpdateDto billUpdateDto)
        {
            var bill = await _billRepository.GetBillByOrderId(billUpdateDto.BillId);
            if (bill == null) return false;

            if (billUpdateDto.PaymentMethod != null)
                bill.PaymentMethod = billUpdateDto.PaymentMethod;
            if (billUpdateDto.PaymentStatus != null)
                bill.PaymentStatus = billUpdateDto.PaymentStatus;

            await _billRepository.UpdateBill(bill);
            return true;
        }

        public async Task<bool> UpdateOrder(int customerId, OrderUpdateDto orderRequestDto)
        {
            var orders = await _orderRepository.GetOrderByUserId(customerId);
            var order = orders.FirstOrDefault(o => o.OrderId == orderRequestDto.OrderId);
            if (order == null) return false;

            order.Status = orderRequestDto.OrderStatus;
            await _orderRepository.UpdateOrder(order);
            return true;
        }
    }
}
