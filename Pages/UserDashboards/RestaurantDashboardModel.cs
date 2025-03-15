using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.UserDashboards
{
    public class RestaurantDashboardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string RestaurantName { get; set; }

        [BindProperty]
        public string ItemName { get; set; }

        [BindProperty]
        public string Price { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public bool ShowAddItemForm { get; set; }

        private readonly string menusDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "Menus");

        public void OnGet(string action = "")
        {
            if (string.IsNullOrEmpty(RestaurantName))
            {
                RestaurantName = "Unknown Restaurant";
                return;
            }

            string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");

            if (System.IO.File.Exists(menuFilePath))
            {
                var lines = System.IO.File.ReadAllLines(menuFilePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(',', 3);
                    if (parts.Length == 3)
                    {
                        MenuItems.Add(new MenuItem
                        {
                            Name = parts[0].Trim(),
                            Price = parts[1].Trim(),
                            Description = parts[2].Trim()
                        });
                    }
                }
            }

            if (action == "add")
            {
                ShowAddItemForm = true;
            }
        }

        public IActionResult OnPost()
        {
            if (string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(ItemName) || string.IsNullOrEmpty(Price) || string.IsNullOrEmpty(Description))
            {
                return Page();
            }

            string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");

            // Ensure the price has a "$" sign
            if (!Price.StartsWith("$"))
            {
                Price = "$" + Price;
            }

            string newItemEntry = $"{ItemName},{Price},{Description}";

            // Append the new item to the menu file
            using (StreamWriter sw = System.IO.File.AppendText(menuFilePath))
            {
                sw.WriteLine(newItemEntry);
            }

            return RedirectToPage("/UserDashboards/RestaurantDashboard", new { restaurantName = RestaurantName });
        }
    }

    public class MenuItem
    {
        public string Name { get; set; }
        public string Price { get; set; }
        public string Description { get; set; }
    }
}
