using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.CreateAccounts
{
    // Handles logic for registering a new restaurant
    public class RegisterRestaurantModel : PageModel
    {
        [BindProperty]
        public string? RestaurantName { get; set; } // Restaurant name input

        [BindProperty]
        public string? Password { get; set; } // Password input

        public string? ErrorMessage { get; set; } // Error message for duplicate restaurant

        private readonly string restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurants.json"); // Path to restaurants.json
        private readonly string menusDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "Menus"); // Path to Menus folder

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            // Input validation
            if (string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("", "Both fields are required.");
                return Page();
            }

            // Create Menus folder if it doesn't exist
            if (!Directory.Exists(menusDirectoryPath))
            {
                Directory.CreateDirectory(menusDirectoryPath);
            }

            Dictionary<string, string> restaurants;

            // Load existing restaurants from restaurants.json
            if (System.IO.File.Exists(restaurantFilePath) && new FileInfo(restaurantFilePath).Length > 0)
            {
                var jsonData = System.IO.File.ReadAllText(restaurantFilePath);
                restaurants = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData) ?? new Dictionary<string, string>();
            }
            else
            {
                restaurants = new Dictionary<string, string>();
            }

            // Check if restaurant already exists
            if (restaurants.ContainsKey(RestaurantName))
            {
                ErrorMessage = "A restaurant with this name already exists.";
                return Page();
            }

            // Store restaurant with hashed password
            restaurants[RestaurantName] = BCrypt.Net.BCrypt.HashPassword(Password);

            // Save updated restaurants.json
            System.IO.File.WriteAllText(restaurantFilePath, JsonSerializer.Serialize(restaurants, new JsonSerializerOptions { WriteIndented = true }));

            // Create empty menu file for new restaurant
            string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");
            if (!System.IO.File.Exists(menuFilePath))
            {
                System.IO.File.Create(menuFilePath).Dispose();
            }

            return RedirectToPage("/UserDashboards/OwnerDashboard");
        }
    }
}
