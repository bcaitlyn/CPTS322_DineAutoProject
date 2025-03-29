using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.Cart
{
    public class CartModel : PageModel
    {
        public CartObj userCart { get; private set; } = new CartObj();
        public void OnPost()
        {
            CartMethods cartMethods = new CartMethods();
            this.userCart = cartMethods.LoadUsersCart()[HttpContext.Session.GetString("Username")];
            Item testItem1 = new Item(0, "Pizza", 5);
            this.userCart.AddItem(testItem1);
        }
    }
}
