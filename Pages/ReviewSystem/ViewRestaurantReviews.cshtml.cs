/*
Programmer: Yevin
Created on: 04/23
Allows Customres to view restaurant reviews.
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DineAuto.Pages.ReviewSystem
{
    public class ViewRestaurantReviewsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string City { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RestaurantName { get; set; }

        public List<RestaurantReview> Reviews { get; set; } = new();

        public class RestaurantReview
        {
            public string Username { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(City) || string.IsNullOrEmpty(RestaurantName))
            {
                return RedirectToPage("/Error");
            }

            string filePath = "Tables/restaurantReviews.json";

            if (!System.IO.File.Exists(filePath))
            {
                return Page(); // No reviews yet
            }

            var json = System.IO.File.ReadAllText(filePath);

            if (!string.IsNullOrWhiteSpace(json))
            {
                var reviewsData = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, List<RestaurantReview>>>>(json)
                                  ?? new();

                if (reviewsData.ContainsKey(City) && reviewsData[City].ContainsKey(RestaurantName))
                {
                    Reviews = reviewsData[City][RestaurantName];
                }
            }

            return Page();
        }
    }
}
