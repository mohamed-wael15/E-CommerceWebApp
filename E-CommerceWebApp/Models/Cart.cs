namespace E_CommerceWebApp.Models
{
    public class Cart
    {
        public int CartId { get; set; }
        public DateTime TimeCreated { get; set; }
        public ICollection<CartItem> CartItems { get; set; }
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }
    }
}
