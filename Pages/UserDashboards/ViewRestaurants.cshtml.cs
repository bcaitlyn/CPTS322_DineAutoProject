using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DineAuto.Pages.UserDashboards
{
    public class ViewRestaurantsModel : PageModel
    {
        public Dictionary<string, List<RestaurantEntry>> Restaurants { get; set; }

        public void OnGet()
        {
            string catalogPath = "Tables/restaurantCatalog.json";
            if (System.IO.File.Exists(catalogPath))
            {
                string json = System.IO.File.ReadAllText(catalogPath);
                Restaurants = JsonSerializer.Deserialize<Dictionary<string, List<RestaurantEntry>>>(json)
                              ?? new Dictionary<string, List<RestaurantEntry>>();
            }
            else
            {
                Restaurants = new Dictionary<string, List<RestaurantEntry>>();
            }
        }

        public class RestaurantEntry
        {
            public string Name { get; set; }
            public string Cuisine { get; set; }
        }
    }
}
