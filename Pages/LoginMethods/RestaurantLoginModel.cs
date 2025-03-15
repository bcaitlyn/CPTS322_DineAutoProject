using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.Login
{
    public class RestaurantLoginModel : PageModel
    {
        [BindProperty]
        public string? RestaurantName { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        public string? ErrorMessage { get; set; } // Holds the error message

        private readonly string restaurantFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurants.json");

        public void OnGet()
        {
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(Password))
            {
                ErrorMessage = "Both fields are required.";
                return Page();
            }

            Dictionary<string, string> restaurants;

            if (System.IO.File.Exists(restaurantFilePath) && new FileInfo(restaurantFilePath).Length > 0)
            {
                var jsonData = System.IO.File.ReadAllText(restaurantFilePath);
                restaurants = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData) ?? new Dictionary<string, string>();
            }
            else
            {
                ErrorMessage = "No registered restaurants found.";
                return Page();
            }

            // Validate restaurant credentials
            if (restaurants.ContainsKey(RestaurantName) && BCrypt.Net.BCrypt.Verify(Password, restaurants[RestaurantName]))
            {
                return RedirectToPage("/UserDashboards/RestaurantDashboard", new { restaurantName = RestaurantName });
            }
            else
            {
                ErrorMessage = "Invalid restaurant name or password.";
                return Page();
            }
        }
    }
}
