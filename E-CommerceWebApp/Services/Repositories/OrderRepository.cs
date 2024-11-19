using E_CommerceWebApp.Data;
using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Order> ConfirmCartToOrderAsync(Cart cart)
        {
            var newOrder = new Order
            {
                OrderStatus = OrderStatus.Waiting,
                TimeCreated = DateTime.Now,
                UserId = cart.UserId,
                OrderItems = cart.CartItems.Select(cartItem => new OrderItem
                {
                    Amount = cartItem.Amount,
                    SinglePrice = cartItem.SinglePrice,
                    ProductID = cartItem.ProductId,
                }).ToList()
            };

            var order = await _context.Orders.AddAsync(newOrder);
            await _context.SaveChangesAsync();
            return order.Entity;
        }
        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == orderId);
        }
        public async Task<IEnumerable<Order>> GetOrdersWithStatusAsync(OrderStatus orderStatus)
        {
            return await _context.Orders.Where(o => o.OrderStatus == orderStatus).ToListAsync();
        }
        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId)
        {
            if (string.IsNullOrEmpty(userId)) return null;
            return await _context.Orders.Where(o => o.UserId == userId).ToListAsync();
        }
        public async Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus newOrderStatus)
        {
            if (orderId < 1) return false;

            var orderToCancel = await GetOrderById(orderId);
            if (orderToCancel is not Order) return false;

            if (newOrderStatus == OrderStatus.Shipping)
                orderToCancel.TimeConfirmed = DateTime.Now;

            orderToCancel.OrderStatus = newOrderStatus;
            await _context.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ConfirmOrderAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, OrderStatus.Shipping);
        }
        public async Task<bool> CancelOrderAsync(int orderId)
        {
            return await UpdateOrderStatusAsync(orderId, OrderStatus.Cancelled);
        }
    }
}
