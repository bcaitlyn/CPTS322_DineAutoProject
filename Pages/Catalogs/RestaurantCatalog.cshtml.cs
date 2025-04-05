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
        public Dictionary<string, List<Restaurant>> Restaurants;
        public RestaurantCatalog catalog = new RestaurantCatalog();

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
         */
        public void OnGet()
        {
            this.Restaurants = catalog.LoadRestaurants();
            
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