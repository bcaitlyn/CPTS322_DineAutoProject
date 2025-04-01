using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.UserDashboards
{
    // Handles displaying and managing a restaurant's menu
    public class RestaurantDashboardModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string? RestaurantName { get; set; } // Restaurant name from URL

        [BindProperty]
        public string? ItemName { get; set; } // New item name input

        [BindProperty]
        public string? Price { get; set; } // New item price input

        [BindProperty]
        public string? Description { get; set; } // New item description input

        [BindProperty]
        public List<string> SelectedItems { get; set; } = new List<string>(); // Selected items for deletion

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>(); // List of menu items
        public bool ShowAddItemForm { get; set; } // Show add item form
        public bool ShowDeleteCheckboxes { get; set; } // Show delete checkboxes

        private readonly string menusDirectoryPath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "Menus"); // Path to Menus folder

        // Handles GET request
        public void OnGet(string action = "")
        {
            if (string.IsNullOrEmpty(RestaurantName))
            {
                RestaurantName = "Unknown Restaurant";
                return;
            }

            // Load menu items from txt file
            string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");

            if (System.IO.File.Exists(menuFilePath))
            {
                var lines = System.IO.File.ReadAllLines(menuFilePath);

                foreach (var line in lines)
                {
                    var parts = line.Split(',', 3); // Split into Name, Price, Description
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

            // Determine which action to show
            if (action == "add")
            {
                ShowAddItemForm = true;
            }
            else if (action == "delete")
            {
                ShowDeleteCheckboxes = true;
            }
        }

        // Handles POST request
        public IActionResult OnPost()
        {
            string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");

            // Add new item
            if (!string.IsNullOrEmpty(ItemName) && !string.IsNullOrEmpty(Price) && !string.IsNullOrEmpty(Description))
            {
                if (!Price.StartsWith("$"))
                {
                    Price = "$" + Price; // Add $ sign if missing
                }

                string newItemEntry = $"{ItemName},{Price},{Description}";

                // Append item to txt file
                using (StreamWriter sw = System.IO.File.AppendText(menuFilePath))
                {
                    sw.WriteLine(newItemEntry);
                }
            }
            // Delete selected items
            else if (SelectedItems.Count > 0)
            {
                if (System.IO.File.Exists(menuFilePath))
                {
                    var lines = System.IO.File.ReadAllLines(menuFilePath);
                    var updatedLines = new List<string>();

                    // Keep lines that are not selected for deletion
                    foreach (var line in lines)
                    {
                        var parts = line.Split(',', 3);
                        if (parts.Length == 3 && !SelectedItems.Contains(parts[0].Trim()))
                        {
                            updatedLines.Add(line);
                        }
                    }

                    // Rewrite txt file
                    System.IO.File.WriteAllLines(menuFilePath, updatedLines);
                }
            }

            // Redirect back to dashboard
            return RedirectToPage("/UserDashboards/RestaurantDashboard", new { restaurantName = RestaurantName });
        }
    }

    // Class to hold menu item details
    public class MenuItem
    {
        public string? Name { get; set; } // Item name
        public string? Price { get; set; } // Item price
        public string? Description { get; set; } // Item description
    }
}
