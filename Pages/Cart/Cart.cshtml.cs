using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.Cart
{
    public class CartModel : PageModel
    {
        public CartObj userCart { get; private set; }
        private Dictionary<string, CartObj> allCarts;
        private CartMethods cartMethods = new CartMethods();
        private OrderMethods orderMethods = new OrderMethods();

        public string Message { get; private set; }

        public CartModel()
        {
            this.allCarts = this.cartMethods.LoadUsersCart();
        }

        public void OnGet()
        {
            string username = "testuser"; // Replace with actual logged-in username

            // Initialize cart if it doesn't exist
            if (!allCarts.ContainsKey(username))
            {
                allCarts[username] = new CartObj();
                cartMethods.SaveUsersCart(allCarts); // Optional: persist empty cart
            }

            userCart = allCarts[username];
        }

        public IActionResult OnPostPlaceOrder()
        {
            string username = "testuser"; // Replace with actual logged-in username

            // Ensure cart exists
            if (!allCarts.ContainsKey(username))
            {
                allCarts[username] = new CartObj();
                cartMethods.SaveUsersCart(allCarts);
            }

            userCart = allCarts[username];

            if (userCart.items.Count == 0)
            {
                Message = "Please add items to cart.";
                return Page();
            }

            // Check if all items are from one restaurant
            var restaurantNames = userCart.items
                .Select(i => i.RestaurantName)
                .Distinct()
                .ToList();

            if (restaurantNames.Count > 1)
            {
                Message = "Please select items from only 1 restaurant.";
                return Page();
            }

            // Create and save order
            var orders = orderMethods.LoadOrders();
            var newOrder = new OrderObj(username, restaurantNames[0], userCart.items);

            if (!orders.ContainsKey(username))
            {
                orders[username] = new List<OrderObj>();
            }
            orders[username].Add(newOrder);
            orderMethods.SaveOrders(orders);

            // Clear cart
            userCart.items.Clear();
            cartMethods.SaveUsersCart(allCarts);

            Message = "Order placed successfully!";
            return Page();
        }
    }
}
