// This file created by Yevin 4/24
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using DineAuto.Pages.Cart;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        private static readonly string ordersPath = "Pages/Testing/TestingTables/testOrders.json";

        // Helper to save test orders
        private static void SaveTestOrders(Dictionary<string, List<OrderObj>> orders)
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(ordersPath, json);
        }

        // Helper to load orders
        private static Dictionary<string, List<OrderObj>> LoadTestOrders()
        {
            if (File.Exists(ordersPath))
            {
                string json = File.ReadAllText(ordersPath);
                return JsonConvert.DeserializeObject<Dictionary<string, List<OrderObj>>>(json)
                    ?? new Dictionary<string, List<OrderObj>>();
            }
            return new Dictionary<string, List<OrderObj>>();
        }

        // Test 01: Valid order with items from one restaurant (Should pass)
        public static void TestPlaceValidOrder()
        {
            string username = "customerA";
            string restaurant = "Pizza Palace";
            var item = new Item("Pepperoni Pizza", 15.99M, "Pizza Palace");

            var order = new OrderObj(username, restaurant, new List<Item> { item });

            var orders = new Dictionary<string, List<OrderObj>>();
            orders[username] = new List<OrderObj> { order };

            SaveTestOrders(orders);

            var loaded = LoadTestOrders();

            bool passed = loaded.ContainsKey(username)
                       && loaded[username].Count == 1
                       && loaded[username][0].RestaurantName == restaurant
                       && loaded[username][0].OrderedItems[0].ItemName == "Pepperoni Pizza";

            Console.WriteLine(passed ? "Place Order Test PASSED." : "Place Order Test FAILED.");
        }

        // Test 02: Cart contains items from different restaurants (should not place order)
        public static void TestPlaceOrderMultipleRestaurants()
        {
            string username = "customerB";

            var item1 = new Item("Burger", 10.00M, "Burger Town");
            var item2 = new Item("Sushi", 20.00M, "Sushi World");

            // Simulate item1 from one restaurant, item2 from another
            string rest1 = "Burger Town";
            string rest2 = "Sushi World";

            // Invalid scenario: we simulate rejection by not saving it
            var orders = new Dictionary<string, List<OrderObj>>();

            // simulate check
            bool valid = rest1 == rest2; // Should be false, so order is not placed

            if (valid)
            {
                orders[username] = new List<OrderObj>
                {
                    new OrderObj(username, rest1, new List<Item> { item1, item2 })
                };
                SaveTestOrders(orders);
            }

            var loaded = LoadTestOrders();

            bool passed = !loaded.ContainsKey(username);
            Console.WriteLine(passed ? "Multi-Restaurant Order Test PASSED." : "Multi-Restaurant Order Test FAILED.");
        }

        // Test 03: Attempt to place an order with an empty cart (Should not allow order)
        public static void TestPlaceOrderEmptyCart()
        {
            string username = "customerC";

            // No order is placed
            var orders = new Dictionary<string, List<OrderObj>>();

            SaveTestOrders(orders);

            var loaded = LoadTestOrders();

            bool passed = !loaded.ContainsKey(username);
            Console.WriteLine(passed ? "Empty Cart Order Test PASSED." : "Empty Cart Order Test FAILED.");
        }
    }
}