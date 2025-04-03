using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using DineAuto.Pages.RestaurantModels;

namespace DineAuto.Pages.UserDashboards
{
    /// <summary>
    /// Model for the Restaurant Dashboard page.
    /// Displays the menu items for a specific restaurant.
    /// </summary>
    public class RestaurantDashboardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string RestaurantName { get; set; } // Holds restaurant name from query

        public List<MenuItem> MenuItems { get; private set; } = new(); // List of menu items

        private readonly string menusFilePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "Menus.json");

        /// <summary>
        /// Loads the restaurant's menu items.
        /// </summary>
        public void OnGet()
        {
            if (string.IsNullOrEmpty(RestaurantName))
            {
                return;
            }

            // Load menus from Menus.json
            if (System.IO.File.Exists(menusFilePath))
            {
                var jsonData = System.IO.File.ReadAllText(menusFilePath);
                var menus = JsonSerializer.Deserialize<Dictionary<string, List<MenuItem>>>(jsonData);

                if (menus != null && menus.ContainsKey(RestaurantName))
                {
                    MenuItems = menus[RestaurantName];
                }
            }
        }
    }
}
