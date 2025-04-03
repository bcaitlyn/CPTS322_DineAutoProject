using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DineAuto.Pages.Restaurant; // Import RestaurantModel class

namespace DineAuto.Pages.CreateAccounts
{
    /// <summary>
    /// Handles restaurant registration logic.
    /// Saves restaurant data and initializes an empty menu in Menus.json.
    /// </summary>
    public class RegisterRestaurantModel : PageModel
    {
        [BindProperty]
        public string RestaurantName { get; set; } // Input restaurant name

        public string Message { get; private set; } // Message for user

        private readonly string restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurants.json");
        private readonly string menusFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "Menus.json");

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(RestaurantName))
            {
                ModelState.AddModelError("", "Restaurant name is required.");
                return Page();
            }

            // Get owner username from session
            string ownerUsername = HttpContext.Session.GetString("Username");
            string role = HttpContext.Session.GetString("Role");

            if (string.IsNullOrEmpty(ownerUsername) || role != "Owner")
            {
                ModelState.AddModelError("", "Session expired or unauthorized access. Please log in again as Owner.");
                return Page();
            }

            // Load existing restaurants
            Dictionary<string, RestaurantModel> restaurants;
            if (System.IO.File.Exists(restaurantFilePath) && new FileInfo(restaurantFilePath).Length > 0)
            {
                var jsonData = System.IO.File.ReadAllText(restaurantFilePath);
                restaurants = JsonSerializer.Deserialize<Dictionary<string, RestaurantModel>>(jsonData) 
                              ?? new Dictionary<string, RestaurantModel>();
            }
            else
            {
                restaurants = new Dictionary<string, RestaurantModel>();
            }

            // Prevent duplicate names
            if (restaurants.ContainsKey(RestaurantName))
            {
                ModelState.AddModelError("", "A restaurant with this name already exists.");
                return Page();
            }

            // Add new restaurant
            var newRestaurant = new RestaurantModel(RestaurantName, ownerUsername);
            restaurants[RestaurantName] = newRestaurant;

            var restaurantJson = JsonSerializer.Serialize(restaurants, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(restaurantFilePath, restaurantJson);

            // Load or create Menus.json
            Dictionary<string, List<string>> menus;
            if (System.IO.File.Exists(menusFilePath) && new FileInfo(menusFilePath).Length > 0)
            {
                var menuData = System.IO.File.ReadAllText(menusFilePath);
                menus = JsonSerializer.Deserialize<Dictionary<string, List<string>>>(menuData) 
                        ?? new Dictionary<string, List<string>>();
            }
            else
            {
                menus = new Dictionary<string, List<string>>();
            }

            // Add empty menu list
            menus[RestaurantName] = new List<string>();
            var menusJson = JsonSerializer.Serialize(menus, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(menusFilePath, menusJson);

            Message = "Restaurant registered successfully!";
            return RedirectToPage("/UserDashboards/OwnerDashboard");
        }
    }
}
