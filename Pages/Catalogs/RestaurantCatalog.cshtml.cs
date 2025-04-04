using Microsoft.AspNetCore.Mvc.RazorPages;
using DineAuto.Pages.Catalogs;
using System;
using Newtonsoft.Json;

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
    }
}