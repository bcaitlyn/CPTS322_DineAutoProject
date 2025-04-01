using System;
using System.Collections.Generic;

namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Represents an order placed by a customer.
    /// Contains order ID, customer username, restaurant name, ordered items, and order date.
    /// </summary>
    public class OrderObj
    {
        public Guid OrderID { get; set; } = Guid.NewGuid(); // Unique order ID
        public string Username { get; set; } // Customer username
        public string RestaurantName { get; set; } // Restaurant name
        public List<Item> OrderedItems { get; set; } // List of ordered items
        public DateTime OrderDate { get; set; } // Date and time order was placed

        /// <summary>
        /// Constructor for OrderObj.
        /// </summary>
        public OrderObj(string username, string restaurantName, List<Item> orderedItems)
        {
            Username = username;
            RestaurantName = restaurantName;
            OrderedItems = orderedItems;
            OrderDate = DateTime.Now;
        }
    }
}
