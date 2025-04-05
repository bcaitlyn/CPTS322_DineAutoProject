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
