using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;

namespace DineAuto.Pages.UserDashboards
{
    public class RestaurantDashboardModel : PageModel
    {
        public string RestaurantName { get; set; }
        public string City { get; set; }
        public List<MenuItem> Menu { get; set; } = new();

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

            return Page();
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

            var restaurant = catalog?[city]?.FirstOrDefault(r =>
                string.Equals(r.Name, restaurantName, StringComparison.OrdinalIgnoreCase));

            if (restaurant == null) return RedirectToPage("/Error");

            restaurant.Menu.Add(new MenuItem
            {
                ItemName = newItemName,
                ItemPrice = newItemPrice
            });

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
        }

        public class MenuItem
        {
            public string ItemName { get; set; }
            public double ItemPrice { get; set; }
        }
    }
}
