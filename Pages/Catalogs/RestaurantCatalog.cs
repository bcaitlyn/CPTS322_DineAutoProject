using Newtonsoft.Json;
using System;
using System.Collections.Generic;

/*
 * Class: RestaurantCatalog
 * Description: Provides the methods necessary to load the restaurants from the
 * restaurantCatalog.json file.
 */
namespace DineAuto.Pages.Catalogs
{
    
    public class RestaurantCatalog
    {
        // Path to the orders.json file
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "restaurantCatalog.json");

        /*
         * Method: LoadRestaurants
         * Description: Loads the restaurants and menus from the restaurantCatalog.json file.
         * and returns a dictionary object of its contents.
         * 
         * Programmer: Caitlyn Boyd
         * Last Modified: 4/3/25
         */
        public Dictionary<string, List<Restaurant>> LoadRestaurants()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, List<Restaurant>>>(json) ?? new Dictionary<string, List<Restaurant>>();
            }
            return new Dictionary<string, List<Restaurant>>();
        }
    }
}