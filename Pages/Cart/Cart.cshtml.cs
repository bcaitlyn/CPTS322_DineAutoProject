using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.Text.Json;
using DineAuto.Pages.Cart;
using DineAuto.Pages.UserDashboards.CustomerDashboard;

namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Backend logic for Cart.cshtml page.
    /// Manages user's cart, order placement, and clearing cart.
    /// </summary>
    public class CartModel : PageModel
    {
        public CartObj userCart { get; private set; } // Current user's cart
        private Dictionary<string, CartObj> allCarts; // All user carts
        private CartMethods cartMethods = new CartMethods(); // Cart helper methods
        private OrderMethods orderMethods = new OrderMethods(); // Order helper methods

        public string Message { get; private set; } // Message displayed to user

        /// <summary>
        /// Constructor: Loads all carts from carts.json
        /// </summary>
        public CartModel()
        {
            this.allCarts = this.cartMethods.LoadUsersCart();
        }

        /// <summary>
        /// Handles GET request to load cart data.
        /// Redirects to login if user not logged in.
        /// </summary>
        public void OnGet()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                Response.Redirect("/login");
                return;
            }

            if (!allCarts.ContainsKey(username))
            {
                allCarts[username] = new CartObj();
                cartMethods.SaveUsersCart(allCarts);
            }

            userCart = allCarts[username];
        }

        /// <summary>
        /// Places an order for the contents of the cart.
        /// Validates cart contents and creates order.
        /// </summary>
        public IActionResult OnPostPlaceOrder()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/login");
            }

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

            // Ensure all items are from one restaurant
            var restaurantNames = userCart.items
                .Select(i => i.RestaurantName)
                .Distinct()
                .ToList();

            if (restaurantNames.Count > 1)
            {
                Message = "Please select items from only 1 restaurant.";
                return Page();
            }

            // Create order
            var orders = orderMethods.LoadOrders();
            var newOrder = new OrderObj(username, restaurantNames[0], userCart.items);

            if (!orders.ContainsKey(username))
            {
                orders[username] = new List<OrderObj>();
            }
            orders[username].Add(newOrder);
            orderMethods.SaveOrders(orders);

            // emily 04/02: payment processing
            // yevin 04/05: bug fix: order placed with $0 in account
            decimal orderTotal = userCart.GetTotal();
            CustomerDashboardModel userFunds = new CustomerDashboardModel();

            if (userFunds.getBalance(username) < orderTotal)
            {
                Message = "Insufficient funds. Please add money to your account.";
                return Page();
            }

            // Funds are sufficient → proceed
            userFunds.modifyFunds(username, orderTotal * -1);

            

            // Clear cart
            userCart.items.Clear();
            cartMethods.SaveUsersCart(allCarts);

            Message = "Order placed successfully!";
            return Page();

        }

        /// <summary>
        /// Clears the contents of the user's cart.
        /// </summary>
        public IActionResult OnPostClearCart()
        {
            string username = HttpContext.Session.GetString("Username");

            if (string.IsNullOrEmpty(username))
            {
                return RedirectToPage("/login");
            }

            if (allCarts.ContainsKey(username))
            {
                userCart = allCarts[username];
                userCart.items.Clear();
                cartMethods.SaveUsersCart(allCarts);
            }

            Message = "Cart has been cleared.";
            return Page();
        }
        public IActionResult OnPostOrderHistory()
        {
            return RedirectToPage("/OrderHistory/OrderHistory");
        }
    }
}
