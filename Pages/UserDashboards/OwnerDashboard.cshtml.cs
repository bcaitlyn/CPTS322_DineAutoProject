using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using DineAuto.Pages.RestaurantModels;

namespace DineAuto.Pages.UserDashboards
{
    /// <summary>
    /// Handles logic for the Owner Dashboard.
    /// Displays all restaurants created by the logged-in owner.
    /// </summary>
    public class OwnerDashboardModel : PageModel
    {
        public List<string> OwnedRestaurants { get; private set; } = new List<string>();
        private readonly string restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurants.json");

        public IActionResult OnGet()
        {
            string ownerUsername = HttpContext.Session.GetString("Username");
            if (string.IsNullOrEmpty(ownerUsername))
            {
                return RedirectToPage("/login");
            }

            if (System.IO.File.Exists(restaurantFilePath) && new FileInfo(restaurantFilePath).Length > 0)
            {
                var jsonData = System.IO.File.ReadAllText(restaurantFilePath);
                var restaurants = JsonSerializer.Deserialize<Dictionary<string, RestaurantModel>>(jsonData) 
                                  ?? new Dictionary<string, RestaurantModel>();

                // Filter restaurants owned by logged-in owner
                OwnedRestaurants = restaurants
                    .Where(r => r.Value.OwnerUsername == ownerUsername)
                    .Select(r => r.Value.RestaurantName)
                    .ToList();
            }

            return Page();
        }
    }
}
