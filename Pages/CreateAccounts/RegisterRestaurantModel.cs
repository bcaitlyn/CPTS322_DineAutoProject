using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.CreateAccounts
{
    public class RegisterRestaurantModel : PageModel
    {
        [BindProperty]
        public string? RestaurantName { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; } // Holds the error message

        private readonly string restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurants.json");
        private readonly string menusDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "Menus");

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(Password))
            {
                ModelState.AddModelError("", "Both fields are required.");
                return Page();
            }

            // Ensure that the Menus directory exists
            if (!Directory.Exists(menusDirectoryPath))
            {
                Directory.CreateDirectory(menusDirectoryPath);
            }

            Dictionary<string, string> restaurants;

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

            // Store hashed password
            restaurants[RestaurantName] = BCrypt.Net.BCrypt.HashPassword(Password);

            // Save updated restaurants.json file
            System.IO.File.WriteAllText(restaurantFilePath, JsonSerializer.Serialize(restaurants, new JsonSerializerOptions { WriteIndented = true }));

            // Create an empty menu TXT file for the restaurant
            string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");
            if (!System.IO.File.Exists(menuFilePath))
            {
                System.IO.File.Create(menuFilePath).Dispose(); // Creates an empty file
            }

            return RedirectToPage("/UserDashboards/OwnerDashboard");
        }
    }
}
