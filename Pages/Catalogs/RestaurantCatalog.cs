using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using DineAuto.Pages.Cart;

using System.Security.Cryptography.X509Certificates;

/*
 * Class: RestaurantCatalog
 * Description: Provides the methods necessary to load the restaurants from the
 * restaurantCatalog.json file.
 */
namespace DineAuto.Pages.Catalogs
{

    public class RestaurantCatalog
    {
        public string SearchTerm { get; set; } = string.Empty; // Emily 4/22: empty string by default until user types and enters in search bar

        // Emily 4/22: takes the search term and matches to any field in the restaurant catalog.
        public Dictionary<string, List<Restaurant>> SearchRestaurants(Dictionary<string, List<Restaurant>> Restaurants)
        {
            if (string.IsNullOrWhiteSpace(SearchTerm))
            {
                // If no filters are active return all restaurants
                return Restaurants;
            }

            foreach (var city in Restaurants.Keys.ToList())
            {
                Restaurants[city] = Restaurants[city]
                    .Where(r =>
                        (string.IsNullOrWhiteSpace(SearchTerm) ||
                        r.Name.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                        r.Cuisine.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                         r.Location.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                         r.Menu.Any(m => m.ItemName.Contains(SearchTerm, StringComparison.OrdinalIgnoreCase) ||
                                        m.ItemPrice.ToString().Contains(SearchTerm))
                        )
                    )
                    .ToList();
            }

            var filteredRestaurants = Restaurants
                .Where(kvp => kvp.Value.Any())
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            return filteredRestaurants;
        }




        // Path to the orders.json file
        private string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurantCatalog.json");

        /*
         * Method: LoadRestaurants
         * Description: Loads the restaurants and menus from the restaurantCatalog.json file.
         * and returns a dictionary object of its contents.
         * 
         * Programmer: Caitlyn Boyd
         * Last Modified: 4/3/25
         */
        public Dictionary<string, List<Restaurant>> LoadRestaurants()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return Newtonsoft.Json.JsonConvert.DeserializeObject<Dictionary<string, List<Restaurant>>>(json) ?? new Dictionary<string, List<Restaurant>>();
            }
            return new Dictionary<string, List<Restaurant>>();
        }

        public List<RestaurantStat>CalculateStats(Dictionary<string, List<Restaurant>> restaurants)
        {
            List<RestaurantStat> stats = new List<RestaurantStat>();
            foreach (var restaurantList in restaurants.Values)
            {
                foreach (var restaurant in restaurantList)
                {
                    stats.Add(CalculateStat(restaurant.Name));
                }    
            }
            return stats;
        }
        public RestaurantStat CalculateStat(string restaurantName)
        {
            // Load orders
            OrderMethods orderMethods = new OrderMethods();
            Dictionary<string, List<OrderObj>> orders = orderMethods.LoadOrders();
            int TotalOrders = 0;
            double TotalRevenue = 0.0;
            bool flag = false;
            RestaurantStat stat;
            // Count orders from this restaurant
            foreach (var orderList in orders.Values)
            {
                foreach (var orderObj in orderList)
                {
                    if (orderObj.RestaurantName == restaurantName)
                    {
                        flag = true;
                        TotalOrders++;
                        
                        foreach (var orderItem in orderObj.OrderedItems)
                        {
                            TotalRevenue = TotalRevenue +  (double)orderItem.ItemPrice;
                        }
                    }
                }
            }
            if (flag)
            {
                stat = new RestaurantStat(TotalOrders, TotalRevenue, restaurantName);
            }
            else
            {
                stat = new RestaurantStat(0, 0.0,  restaurantName);
            }
            return stat;

        }
       
    }
}