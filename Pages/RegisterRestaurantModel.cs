using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages
{
    public class RegisterRestaurantModel : PageModel
    {
        [BindProperty]
        public string? RestaurantName { get; set; }

        [BindProperty]
        public string? Password { get; set; }

        private readonly string filePath = "Tables/restaurants.json";

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

            Dictionary<string, string> restaurants;

            if (System.IO.File.Exists(filePath) && new FileInfo(filePath).Length > 0)
            {
                var jsonData = System.IO.File.ReadAllText(filePath);
                restaurants = JsonSerializer.Deserialize<Dictionary<string, string>>(jsonData) ?? new Dictionary<string, string>();
            }
            else
            {
                restaurants = new Dictionary<string, string>();
            }

            // Store hashed password
            restaurants[RestaurantName] = BCrypt.Net.BCrypt.HashPassword(Password);

            System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(restaurants, new JsonSerializerOptions { WriteIndented = true }));

            return RedirectToPage("/UserDashboards/OwnerDashboard");
        }
    }
}
