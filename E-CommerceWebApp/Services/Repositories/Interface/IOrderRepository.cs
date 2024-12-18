﻿using E_CommerceWebApp.Models;

namespace E_CommerceWebApp.Services.Repositories.Interface
{
    public interface IOrderRepository
    {
        public Task<Order> ConfirmCartToOrderAsync(Cart cart);
        public Task<Order>? GetOrderById(int orderId);
        public Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        public Task<IEnumerable<Order>> GetOrdersWithStatusAsync(OrderStatus newOrderStatus);
        public Task<bool> UpdateOrderStatusAsync(int orderId, OrderStatus orderStatus);
        public Task<bool> ConfirmOrderAsync(int orderId);
        public Task<bool> CancelOrderAsync(int orderId);
    }
}
