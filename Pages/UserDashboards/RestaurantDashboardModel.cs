/*
Restaurant Dashboard to view each restaurants items
Created by : Yevin
Last Worked on : 04/05
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using DineAuto.Pages.Catalogs;

namespace DineAuto.Pages.UserDashboards
{
    public class RestaurantDashboardModel : PageModel
    {
        public string RestaurantName { get; set; }
        public string City { get; set; }
        public List<MenuItem> Menu { get; set; } = new();

        // Yevin 4/23 Owners can view customer reviews on their restaurants
        public class RestaurantReview
        {
            public string Username { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
            public string? OwnerReply { get; set; }
        }

        public List<RestaurantReview> Reviews { get; set; } = new();
        [BindProperty] public string ReplyText { get; set; }
        [BindProperty] public int ReplyIndex { get; set; }


        public IActionResult OnGet(string city, string name)
        {
            if (string.IsNullOrWhiteSpace(city) || string.IsNullOrWhiteSpace(name))
            {
                return RedirectToPage("/Error");
            }

            City = city;
            RestaurantName = name;

            string catalogPath = "Tables/restaurantCatalog.json";

            if (!System.IO.File.Exists(catalogPath))
            {
                return RedirectToPage("/Error");
            }

            var catalogJson = System.IO.File.ReadAllText(catalogPath);
            var catalog = JsonSerializer.Deserialize<Dictionary<string, List<RestaurantEntry>>>(catalogJson);

            if (catalog == null || !catalog.ContainsKey(city))
            {
                return RedirectToPage("/Error");
            }

            var restaurant = catalog[city]
                .FirstOrDefault(r => string.Equals(r.Name, name, System.StringComparison.OrdinalIgnoreCase));

            if (restaurant == null)
            {
                return RedirectToPage("/Error");
            }

            Menu = restaurant.Menu ?? new List<MenuItem>();
            

            string reviewPath = "Tables/restaurantReviews.json";
            if (System.IO.File.Exists(reviewPath))
            {
                var json = System.IO.File.ReadAllText(reviewPath);
                var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<RestaurantReview>>>>(json);
                if (data != null && data.ContainsKey(City) && data[City].ContainsKey(RestaurantName))
                {
                    Reviews = data[City][RestaurantName];
                }
            }

            return Page();
        }

        public IActionResult OnPostReply(string city, string restaurantName)
        {
            string path = "Tables/restaurantReviews.json";
            var json = System.IO.File.ReadAllText(path);
            var data = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<RestaurantReview>>>>(json);

            if (data != null &&
                data.ContainsKey(city) &&
                data[city].ContainsKey(restaurantName) &&
                ReplyIndex >= 0 &&
                ReplyIndex < data[city][restaurantName].Count)
            {
                data[city][restaurantName][ReplyIndex].OwnerReply = ReplyText;
                System.IO.File.WriteAllText(path, JsonSerializer.Serialize(data, new JsonSerializerOptions { WriteIndented = true }));
            }

            return RedirectToPage(new { city, name = restaurantName });
        }
        
        public IActionResult OnPostAddItem(string city, string restaurantName, string newItemName, double newItemPrice)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(restaurantName) ||
                string.IsNullOrEmpty(newItemName) || newItemPrice <= 0)
            {
                return RedirectToPage("/Error");
            }

            string catalogPath = "Tables/restaurantCatalog.json";
            if (!System.IO.File.Exists(catalogPath)) return RedirectToPage("/Error");

            var json = System.IO.File.ReadAllText(catalogPath);
            var catalog = JsonSerializer.Deserialize<Dictionary<string, List<RestaurantEntry>>>(json);

            var restaurantList = catalog?[city];
            if (restaurantList == null) return RedirectToPage("/Error");

            var restaurant = restaurantList.FirstOrDefault(r =>
                string.Equals(r.Name, restaurantName, StringComparison.OrdinalIgnoreCase));

            if (restaurant == null) return RedirectToPage("/Error");

            // Add new item
            restaurant.Menu ??= new List<MenuItem>();
            restaurant.Menu.Add(new MenuItem
            {
                ItemName = newItemName,
                ItemPrice = newItemPrice
            });

            // Replace the original object in the list to preserve all fields (especially OwnerUsername)
            int index = restaurantList.FindIndex(r => r.Name == restaurant.Name);
            if (index >= 0)
            {
                restaurantList[index] = restaurant;
            }


            string updated = JsonSerializer.Serialize(catalog, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(catalogPath, updated);

            return RedirectToPage(new { city = city, name = restaurantName });
        }

        public IActionResult OnPostDeleteItems(string city, string restaurantName, List<string> itemsToDelete)
        {
            if (string.IsNullOrEmpty(city) || string.IsNullOrEmpty(restaurantName) || itemsToDelete == null)
                return RedirectToPage(new { city = city, name = restaurantName });

            string catalogPath = "Tables/restaurantCatalog.json";
            if (!System.IO.File.Exists(catalogPath)) return RedirectToPage("/Error");

            var json = System.IO.File.ReadAllText(catalogPath);
            var catalog = JsonSerializer.Deserialize<Dictionary<string, List<RestaurantEntry>>>(json);

            var restaurant = catalog?[city]?.FirstOrDefault(r =>
                string.Equals(r.Name, restaurantName, StringComparison.OrdinalIgnoreCase));

            if (restaurant == null) return RedirectToPage("/Error");

            restaurant.Menu.RemoveAll(item => itemsToDelete.Contains(item.ItemName));

            // Preserve updated restaurant back into the list
            int index = catalog[city].FindIndex(r => r.Name == restaurant.Name);
            if (index >= 0)
            {
                catalog[city][index] = restaurant;
            }


            string updated = JsonSerializer.Serialize(catalog, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(catalogPath, updated);

            return RedirectToPage(new { city = city, name = restaurantName });
        }

        public class RestaurantEntry
        {
            public string Name { get; set; }
            public string Cuisine { get; set; }
            public string Location { get; set; }
            public List<MenuItem> Menu { get; set; }
            public string OwnerUsername { get; set; }
            public int TotalOrders { get; set; }
        }

        public class MenuItem
        {
            public string ItemName { get; set; }
            public double ItemPrice { get; set; }
        }
    }
}
