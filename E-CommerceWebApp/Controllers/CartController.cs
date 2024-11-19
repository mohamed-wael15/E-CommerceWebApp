using E_CommerceWebApp.Services;
using E_CommerceWebApp.Services.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace E_CommerceWebApp.Controllers
{
    [Authorize]
    [UserCartFilter]
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
        }
        public IActionResult Index(int cartId)
        {
            var userCart = _cartRepository.GetCompleteUserCart(cartId);

            return View(userCart);
        }

        // for product card
        [HttpPost]
        public IActionResult UpdateCartItem(int id, int cartId)
        {
            bool success = true;
            try
            {
                _cartRepository.AddOrUpdateCartItem(cartId, id);
            }
            catch (Exception ex)
            {
                success = false;
            }
            return Json(new { success });
        }

        public IActionResult UpdateCartItemAmount(int itemID, int amount)
        {
            try
            {
                _cartRepository.UpdateCartItemAmount(itemID, amount);
            }
            catch (Exception ex)
            {

            }

            return RedirectToAction(nameof(Index));
        }

        public IActionResult RemoveCartItem(int id)
        {
            _cartRepository.RemoveCartItem(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
