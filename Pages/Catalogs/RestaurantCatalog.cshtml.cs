using Microsoft.AspNetCore.Mvc.RazorPages;
using DineAuto.Pages.Catalogs;
using System;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Mvc;
using DineAuto.Pages.Cart;

/*
 * Class: RestaurantCatalog Model
 * Description: The page model for the restaurant catalog page. Loads all the restaurants
 * for use in the RestaurantCatalog.cshtml file.
 * 
 * Programmer: Caitlyn Boyd
 * Last Modified: 4/3/25
 */
namespace DineAuto.Pages.Catalogs
{
    public class RestaurantCatalogModel : PageModel
    {
        [BindProperty]
        public RestaurantCatalog catalog { get; set; } = new RestaurantCatalog();

        public Dictionary<string, List<Restaurant>> Restaurants { get; set; } 

        [BindProperty]
        public string ItemName { get; set; }

        [BindProperty]
        public decimal ItemPrice { get; set; }

        [BindProperty]
        public string RestaurantName { get; set; }

        /*
         * Method: OnGet
         * Description: Loads the restaurant catalog into the dictionary Restaurants.
         * 
         * Programmer: Caitlyn Boyd
         * Last Modified: 4/3/25
         * 
         * Modified 4/22 by Emily to filter result of loadrestaurants with current search input.
         */
        public void OnGet()
        {
            Restaurants = catalog.LoadRestaurants();
            Restaurants = catalog.SearchRestaurants(Restaurants);
            
        }

        public IActionResult OnPostSearch()
        {
            Restaurants = catalog.LoadRestaurants();
            Restaurants = catalog.SearchRestaurants(Restaurants);
            return Page();
        }

        public IActionResult OnPostReset()
        {
            catalog.SearchTerm = string.Empty;
            Restaurants = catalog.LoadRestaurants();
            
            return Page();
        }

        public void OnPostAddItem()
        {
            this.Restaurants = catalog.LoadRestaurants();
            CartMethods cartMethods = new CartMethods();
            Dictionary<string, CartObj> allCarts = cartMethods.LoadUsersCart();
            Item item = new Item(this.ItemName, this.ItemPrice, this.RestaurantName, new Guid());
            allCarts[HttpContext.Session.GetString("Username")].AddItem(item);
            cartMethods.SaveUsersCart(allCarts);

        }

        // Yevin 4/23 get average ratings for restaurant.
        public string GetRestaurantAverageRating(string city, string restaurantName)
        {
            string path = "Tables/restaurantReviews.json";

            if (!System.IO.File.Exists(path)) return "No reviews";

            var json = System.IO.File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(json))
            {
                var reviewsData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, List<DineAuto.Pages.ReviewSystem.ViewRestaurantReviewsModel.RestaurantReview>>>>(json);
                if (reviewsData != null &&
                    reviewsData.ContainsKey(city) &&
                    reviewsData[city].ContainsKey(restaurantName))
                {
                    var reviews = reviewsData[city][restaurantName];
                    if (reviews.Count > 0)
                    {
                        double avg = reviews.Average(r => r.Rating);
                        return $"{avg:F1} / 5";
                    }
                }
            }

            return "No reviews";
        }

        // Yevin 4/23 get average ratings for restaurant items.
        public string GetItemAverageRating(string city, string restaurantName, string itemName)
        {
            string path = "Tables/itemReviews.json";

            if (!System.IO.File.Exists(path)) return "No reviews";

            var json = System.IO.File.ReadAllText(path);

            if (!string.IsNullOrWhiteSpace(json))
            {
                var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<string, Dictionary<string, List<DineAuto.Pages.ReviewSystem.ViewItemReviewsModel.ItemReview>>>>>(json);
                if (data != null &&
                    data.ContainsKey(city) &&
                    data[city].ContainsKey(restaurantName) &&
                    data[city][restaurantName].ContainsKey(itemName))
                {
                    var reviews = data[city][restaurantName][itemName];
                    if (reviews.Count > 0)
                    {
                        double avg = reviews.Average(r => r.Rating);
                        return $"{avg:F1} / 5";
                    }
                }
            }

            return "No reviews";
        }
    }
}