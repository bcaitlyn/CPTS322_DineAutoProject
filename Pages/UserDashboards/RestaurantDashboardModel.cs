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

        [BindProperty]
        public List<string> SelectedItems { get; set; } = new List<string>();

        public List<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
        public bool ShowAddItemForm { get; set; }
        public bool ShowDeleteCheckboxes { get; set; }

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
            else if (action == "delete")
            {
                ShowDeleteCheckboxes = true;
            }
        }

        public IActionResult OnPost()
        {
            if (!string.IsNullOrEmpty(ItemName) && !string.IsNullOrEmpty(Price) && !string.IsNullOrEmpty(Description))
            {
                string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");

                if (!Price.StartsWith("$"))
                {
                    Price = "$" + Price;
                }

                string newItemEntry = $"{ItemName},{Price},{Description}";

                using (StreamWriter sw = System.IO.File.AppendText(menuFilePath))
                {
                    sw.WriteLine(newItemEntry);
                }
            }
            else if (SelectedItems.Count > 0)
            {
                string menuFilePath = Path.Combine(menusDirectoryPath, $"{RestaurantName}Menu.txt");

                if (System.IO.File.Exists(menuFilePath))
                {
                    var lines = System.IO.File.ReadAllLines(menuFilePath);
                    var updatedLines = new List<string>();

                    foreach (var line in lines)
                    {
                        var parts = line.Split(',', 3);
                        if (parts.Length == 3 && !SelectedItems.Contains(parts[0].Trim()))
                        {
                            updatedLines.Add(line);
                        }
                    }

                    System.IO.File.WriteAllLines(menuFilePath, updatedLines);
                }
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
