/*
Programmer: Yevin
Created on: 04/23
Allows customers to view reviews for each item in restaurant
*/

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Text.Json;

namespace DineAuto.Pages.ReviewSystem
{
    public class ViewItemReviewsModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string City { get; set; }

        [BindProperty(SupportsGet = true)]
        public string RestaurantName { get; set; }

        [BindProperty(SupportsGet = true)]
        public string ItemName { get; set; }

        public List<ItemReview> Reviews { get; set; } = new();

        public class ItemReview
        {
            public string Username { get; set; }
            public int Rating { get; set; }
            public string Comment { get; set; }
        }

        public IActionResult OnGet()
        {
            if (string.IsNullOrEmpty(City) || string.IsNullOrEmpty(RestaurantName) || string.IsNullOrEmpty(ItemName))
            {
                return RedirectToPage("/Error");
            }

            string path = "Tables/itemReviews.json";

            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);

                if (!string.IsNullOrWhiteSpace(json))
                {
                    var allReviews = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, List<ItemReview>>>>>(json)
                                     ?? new();

                    if (allReviews.ContainsKey(City) &&
                        allReviews[City].ContainsKey(RestaurantName) &&
                        allReviews[City][RestaurantName].ContainsKey(ItemName))
                    {
                        Reviews = allReviews[City][RestaurantName][ItemName];
                    }
                }
            }

            return Page();
        }
    }
}