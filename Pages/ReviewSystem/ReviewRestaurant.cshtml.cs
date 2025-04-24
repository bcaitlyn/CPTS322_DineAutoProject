/* 
Programmer : Yevin
Created on: 04/23
Allows users to log in as customers and then review restaurants and items. 
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace DineAuto.Pages.ReviewSystem
{
    public class ReviewRestaurantModel : PageModel
    {
        [BindProperty]
        public int Rating { get; set; }

        [BindProperty]
        public string Comment { get; set; } = string.Empty;

        [BindProperty(SupportsGet = true)]
        public string RestaurantName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string City { get; set; }

        public string Message { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(City))
            {
                return RedirectToPage("/Error");
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (HttpContext.Session.GetString("Username") is not string username)
            {
                return RedirectToPage("/Login"); // Adjust path if needed
            }

            string filePath = "Tables/restaurantReviews.json";
            var newReview = new RestaurantReview
            {
                Username = username,
                Rating = Rating,
                Comment = Comment
            };

            var reviewsData = new Dictionary<string, Dictionary<string, List<RestaurantReview>>>();

            if (System.IO.File.Exists(filePath))
            {
                var json = System.IO.File.ReadAllText(filePath);
                reviewsData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<RestaurantReview>>>>(json)
                              ?? new();
            }

            if (!reviewsData.ContainsKey(City))
                reviewsData[City] = new Dictionary<string, List<RestaurantReview>>();

            if (!reviewsData[City].ContainsKey(RestaurantName))
                reviewsData[City][RestaurantName] = new List<RestaurantReview>();

            // Preserve existing reviews
            var existingReviews = reviewsData[City][RestaurantName];
            existingReviews.Add(new RestaurantReview
            {
                Username = username,
                Rating = Rating,
                Comment = Comment,
                OwnerReply = null // Explicitly preserve reply structure
            });

            // Reassign and serialize
            reviewsData[City][RestaurantName] = existingReviews;

            System.IO.File.WriteAllText(filePath, JsonSerializer.Serialize(reviewsData, new JsonSerializerOptions { WriteIndented = true }));


            Message = "Review submitted successfully!";
            return Page();
        }

        public class RestaurantReview
        {
            public string Username { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
            public string? OwnerReply { get; set; }
        }
    }
}