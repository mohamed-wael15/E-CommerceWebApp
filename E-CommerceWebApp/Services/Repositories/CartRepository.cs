using E_CommerceWebApp.Data;
using E_CommerceWebApp.Models;
using E_CommerceWebApp.Services.Repositories.Interface;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceWebApp.Services.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Cart GetUserCart(string userId)
            => _context.Carts.FirstOrDefault(c => c.UserId == userId);

        public Cart CreateUserCart(string userId)
        {
            if (GetUserCart(userId) != null)
                return null;
            var newCart = new Cart
            {
                UserId = userId,
                TimeCreated = DateTime.Now,
                CartItems = new List<CartItem>()
            };

            _context.Carts.Add(newCart);
            _context.SaveChanges();
            return newCart;
        }
        public Cart GetCompleteUserCart(int cartId)
        {
            var completeUserCart = _context.Carts
                                .Where(c => c.CartId == cartId)
                                .Include(c => c.CartItems)
                                    .ThenInclude(ci => ci.Product)
                                        .ThenInclude(p => p.ProductImage)
                                .Single();

            return completeUserCart;
        }
        public bool ClearCartItems(int cartId)
        {
            if (cartId == null || cartId < 1)
                return false;

            var userCart = GetCompleteUserCart(cartId);

            userCart.CartItems.Clear();

            _context.SaveChanges();

            return true;
        }
        public IEnumerable<CartItem> GetAllCartItems(int cartId)
        {
            return _context.CartItems
               .Where(ci => ci.CartId == cartId)
                   .Include(p => p.Product)
                       .ThenInclude(im => im.ProductImage)
                .ToList();
        }
        public CartItem GetCartItemByID(int itemId)
            => _context.CartItems.FirstOrDefault(i => i.CartItemId == itemId);

        public void AddOrUpdateCartItem(int cartId, int productId)
        {
            var existingCartItem = _context.CartItems
                                    .FirstOrDefault(i => i.ProductId == productId && i.CartId == cartId);

            if (existingCartItem == null)
            {
                _context.Set<CartItem>().Add(new CartItem
                {
                    ProductId = productId,
                    Amount = 1,
                    SinglePrice = 0, // todo delete this prop
                    CartId = cartId,
                });
            }
            else
            {
                existingCartItem.Amount += 1;
            }

            _context.SaveChanges();
        }

        public void UpdateCartItemAmount(int cartItemId, int amount)
        {
            var existingCartItem = GetCartItemByID(cartItemId);

            if (existingCartItem != null && amount >= 1)
            {
                existingCartItem.Amount = amount;

                _context.CartItems.Update(existingCartItem);

                _context.SaveChanges();
            }
        }
        public void RemoveCartItem(int cartItemId)
        {
            var existingCartItem = GetCartItemByID(cartItemId);
            if (existingCartItem != null)
            {
                _context.CartItems.Remove(existingCartItem);
                _context.SaveChanges();
            }
        }

    }
}

