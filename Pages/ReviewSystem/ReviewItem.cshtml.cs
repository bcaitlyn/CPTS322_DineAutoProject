/*
Programmer: Yevin
Date Created: 04/23
Allows Customer to review selected items from restaurants
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DineAuto.Pages.ReviewSystem
{
    public class ReviewItemModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string City { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RestaurantName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ItemName { get; set; }

        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string Comment { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public class ItemReview
        {
            public string Username { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(ItemName))
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("Username") is not string username)
            {
                return RedirectToPage("/Login");
            }

            string path = "Tables/itemReviews.json";
            var newReview = new ItemReview
            {
                Username = username,
                Rating = Rating,
                Comment = Comment
            };

            // New structure: City -> Restaurant -> Item -> Reviews
            var allReviews = new Dictionary<string, Dictionary<string, Dictionary<string, List<ItemReview>>>>();

            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                if (!string.IsNullOrWhiteSpace(json))
                {
                    allReviews = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, List<ItemReview>>>>>(json)
                                ?? new();
                }
            }

            if (!allReviews.ContainsKey(City))
                allReviews[City] = new();

            if (!allReviews[City].ContainsKey(RestaurantName))
                allReviews[City][RestaurantName] = new();

            if (!allReviews[City][RestaurantName].ContainsKey(ItemName))
                allReviews[City][RestaurantName][ItemName] = new();

            allReviews[City][RestaurantName][ItemName].Add(newReview);

            var options = new JsonSerializerOptions { WriteIndented = true };
            System.IO.File.WriteAllText(path, JsonSerializer.Serialize(allReviews, options));

            Message = "Item review submitted successfully!";
            return Page();
        }
    }
}