using E_CommerceWebApp.Models;

namespace E_CommerceWebApp.Services.Repositories.Interface
{
    public interface ICartRepository
    {
        // Cart
        public Cart GetUserCart(string userId);
        public Cart CreateUserCart(string userId);
        public Cart GetCompleteUserCart(int cartId);
        public bool ClearCartItems(int cartId);
        // CartItems
        public IEnumerable<CartItem> GetAllCartItems(int cartId);
        public CartItem GetCartItemByID(int itemId);
        public void AddOrUpdateCartItem(int cartId, int productId);
        public void UpdateCartItemAmount(int cartItemId, int amount);
        public void RemoveCartItem(int cartItemId);
    }
}
