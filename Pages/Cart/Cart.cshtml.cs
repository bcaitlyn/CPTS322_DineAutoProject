using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.Cart
{
    public class CartModel : PageModel
    {
        public CartObj userCart { get; private set; }
        private Dictionary<string, CartObj> allCarts;
        private CartMethods cartMethods = new CartMethods();

        public CartModel()
        {
            this.allCarts = this.cartMethods.LoadUsersCart();
            

        }
        public void OnGet()
        {
            this.userCart = this.allCarts[HttpContext.Session.GetString("Username")];
        }
        public void OnPostRemove(Guid id)
        {
            this.userCart = this.allCarts[HttpContext.Session.GetString("Username")];
            this.userCart.RemoveItem(id);
            this.allCarts[HttpContext.Session.GetString("Username")] = this.userCart;
            this.cartMethods.SaveUsersCart(this.allCarts);
            

        }
    }
}
