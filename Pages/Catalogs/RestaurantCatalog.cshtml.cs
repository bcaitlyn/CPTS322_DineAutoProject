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
    }
}