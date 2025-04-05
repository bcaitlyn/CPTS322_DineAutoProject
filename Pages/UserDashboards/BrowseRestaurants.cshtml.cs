using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace DineAuto.Pages.UserDashboards
{
    public class BrowseRestaurantsModel : PageModel
    {
        public List<string> RestaurantNames { get; set; } = new();

        public void OnGet()
        {
            string restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurants.json");

            if (System.IO.File.Exists(restaurantFilePath))
            {
                var json = System.IO.File.ReadAllText(restaurantFilePath);
                var restaurantDict = JsonSerializer.Deserialize<Dictionary<string, string>>(json);

                if (restaurantDict != null)
                {
                    RestaurantNames = new List<string>(restaurantDict.Keys);
                }
            }
        }
    }
}
