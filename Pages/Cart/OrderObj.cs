using System;
using System.Collections.Generic;

namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Order object stores ordered items and customer details.
    /// </summary>
    public class OrderObj
    {
        public Guid OrderID { get; set; } = Guid.NewGuid();
        public string Username { get; set; }
        public string RestaurantName { get; set; }
        public List<Item> OrderedItems { get; set; }
        public DateTime OrderDate { get; set; }

        public OrderObj(string username, string restaurantName, List<Item> orderedItems)
        {
            Username = username;
            RestaurantName = restaurantName;
            OrderedItems = orderedItems;
            OrderDate = DateTime.Now;
        }
    }
}
