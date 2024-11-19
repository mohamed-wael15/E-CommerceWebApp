namespace E_CommerceWebApp.Models
{
    public class Order
    {
        public int OrderId { get; set; }
        public DateTime TimeCreated { get; set; } = DateTime.Now;
        public OrderStatus OrderStatus { get; set; }
        public DateTime TimeConfirmed { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }
    }

    public enum OrderStatus
    {
        Waiting,
        Confirming,
        Shipping,
        Delayed,
        Cancelled,
        Delivered
    }
}
