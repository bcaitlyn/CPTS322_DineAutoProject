/*
Register Restaurant Method
Programmer : Yevin
Last Worked on : 04/05
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Collections.Generic;

namespace DineAuto.Pages.RegisterRestaurants
{
    public class RegisterRestaurantModel : PageModel
    {
        [BindProperty]
        public string RestaurantName { get; set; }

        [BindProperty]
        public string Cuisine { get; set; }

        [BindProperty]
        public string City { get; set; }

        [BindProperty]
        public string State { get; set; }

        public string Message { get; set; }

        public bool IsSuccess {get; set;} // Checks if restaurant was created successfully

        public bool AlreadyExists {get; set;} // Check if restaurant already exists.

        private readonly string catalogPath = "Tables/restaurantCatalog.json";

        public void OnGet()
        {
        }

        public void OnPost()
        {
            // Check if a field is left empty
            if (string.IsNullOrEmpty(RestaurantName) ||
                string.IsNullOrEmpty(Cuisine) ||
                string.IsNullOrEmpty(City) ||
                string.IsNullOrEmpty(State))
            {
                Message = "All fields are required.";
                IsSuccess = false;
                return;
            }

            Dictionary<string, List<RestaurantEntry>> catalog;
            string formattedLocation = $"{City}, {State}";

            // Check list of restaurants
            if (System.IO.File.Exists(catalogPath))
            {
                string json = System.IO.File.ReadAllText(catalogPath);
                catalog = JsonSerializer.Deserialize<Dictionary<string, List<RestaurantEntry>>>(json)
                          ?? new Dictionary<string, List<RestaurantEntry>>();
            }
            else
            {
                catalog = new Dictionary<string, List<RestaurantEntry>>();
            }

            if (!catalog.ContainsKey(City)) // Check if city exists
            {

                catalog[City] = new List<RestaurantEntry>();
            }
            else // check if restaurants already registered in city
            {
                AlreadyExists = catalog[City].Any(newRestaurant => string.Equals(newRestaurant.Name, RestaurantName, StringComparison.OrdinalIgnoreCase));

                if (AlreadyExists == true)
                {
                    Message = "Restaurant Already Exists";
                    IsSuccess = false;
                    return;
                }
            }

            string ownerUsername = HttpContext.Session.GetString("Username");

            // Add new restaurant
            catalog[City].Add(new RestaurantEntry
            {
                Name = RestaurantName,
                Cuisine = Cuisine,
                Location = formattedLocation,
                OwnerUsername = ownerUsername,
                Menu = new List<MenuItem>()
            });

            string updatedJson = JsonSerializer.Serialize(catalog, new JsonSerializerOptions { WriteIndented = true });
            System.IO.File.WriteAllText(catalogPath, updatedJson);

            Message = "Restaurant registered successfully!";
            IsSuccess = true;
        }

        public class RestaurantEntry
        {
            public string Name { get; set; }
            public string Cuisine { get; set; }
            public string Location { get; set; }
            public List<MenuItem> Menu { get; set; }
            public string OwnerUsername {get; set;}
        }

        public class MenuItem
        {
            public string ItemName { get; set; }
            public double ItemPrice { get; set; }
        }
    }
}
