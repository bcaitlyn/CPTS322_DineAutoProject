/*
Allowes owner to see dashboard of restaurants they have created
Created by : Yevin
Last Worked on : 04/24
// 04/24: CB added support for calculating and displaying restaurant stats.
*/

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Http;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Routing;
using DineAuto.Pages.Catalogs;

namespace DineAuto.Pages.UserDashboards
{
    public class ViewRestaurantsModel : PageModel
    {
        public Dictionary<string, List<RestaurantEntry>> Restaurants { get; set; }
        // CB 4/24/25: added a catalog object to calculate statistics
        public RestaurantCatalog Catalog = new RestaurantCatalog();
        public List<RestaurantStat> Stats;

        public void OnGet()
        {
            string currentOwner = HttpContext.Session.GetString("Username");

            string catalogPath = "Tables/restaurantCatalog.json";
            Dictionary<string, List<RestaurantEntry>> fullCatalog;

            if (System.IO.File.Exists(catalogPath))
            {
                string json = System.IO.File.ReadAllText(catalogPath);
                fullCatalog = JsonSerializer.Deserialize<Dictionary<string, List<RestaurantEntry>>>(json)
                              ?? new Dictionary<string, List<RestaurantEntry>>();
            }
            else
            {
                fullCatalog = new Dictionary<string, List<RestaurantEntry>>();
            }

            // Filter to show only restaurants owned by this user
            Restaurants = fullCatalog
                .Where(city => city.Value.Any(r => r.OwnerUsername == currentOwner))
                .ToDictionary(
                    city => city.Key,
                    city => city.Value.Where(r => r.OwnerUsername == currentOwner).ToList()
                );

            // Modified by Caitlyn Boyd: Does the same as the above but uses the Dictionary<string, List<Restuarant>> instead of restaurantEntry
            Dictionary<string, List<Restaurant>> restaurantDict = Catalog.LoadRestaurants();
            restaurantDict = restaurantDict
                .Where(city => city.Value.Any(r => r.OwnerUsername == currentOwner))
                .ToDictionary(
                    city => city.Key,
                    city => city.Value.Where(r => r.OwnerUsername == currentOwner).ToList()
                );

            Stats = Catalog.CalculateStats(restaurantDict);

        }

        public class RestaurantEntry
        {
            public string Name { get; set; }
            public string Cuisine { get; set; }
            public string Location { get; set; }
            public string OwnerUsername { get; set; }
        }
    }
}
