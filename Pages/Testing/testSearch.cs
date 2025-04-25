using DineAuto.Pages;
using DineAuto.Pages.Catalogs;
using DineAuto.Pages.CreateAccounts;
using DineAuto.Pages.UserDashboards;
using DineAuto.Pages.UserDashboards.CustomerDashboard;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        //test: search for something I know is in the database
        // this test should return only the item and restaurant I manually add here.
        // expected result: file should return only one dictionary entry, matching the search term.
        // actual result: matches expected result.
        public static void TestSearch_03a()
        {
            const string testCity = "testCity";

            RestaurantCatalog testCatalog = new RestaurantCatalog
            {
                SearchTerm = testCity
            };

            PrintItem testItem = new PrintItem("itemTest", 1.00m);
            Restaurant testRestaurant = new Restaurant("res", "american", testCity, new List<PrintItem>{testItem }, "testOwner");

            Dictionary<string, List<Restaurant>> restaurants = new Dictionary<string, List<Restaurant>>
            {
                [testCity] = new List<Restaurant> { testRestaurant }
            };

            Dictionary<string, List<Restaurant>> searchedRestaurants = testCatalog.SearchRestaurants(restaurants);

            if (searchedRestaurants.ContainsKey(testCity) && searchedRestaurants.Count == 1)
            {
                Console.WriteLine("Search test 1 PASSED");
            }
            else if (!searchedRestaurants.ContainsKey(testCity))
            {
                Console.WriteLine("Search test 1 FAILED (doesn't contain key)");
            }
            else if (searchedRestaurants.Count != 1)
            {
                Console.WriteLine("Search test 1 FAILED (not 1)");
            }
            else
            {
                Console.WriteLine("Search test 1 FAILED (unexpected error)");
            }
        }


        //Test: there should be zero results for a search term that doesn't match/substring match anythin in the catalog.
        public static void TestSearch_03b()
        {
            const string testCity = "testCity";

            RestaurantCatalog testCatalog = new RestaurantCatalog
            {
                SearchTerm = "Fakesearch"
            };

            PrintItem testItem = new PrintItem("itemTest", 1.00m);
            Restaurant testRestaurant = new Restaurant("res", "american", testCity, new List<PrintItem> { testItem }, "testOwner");

            Dictionary<string, List<Restaurant>> restaurants = new Dictionary<string, List<Restaurant>>
            {
                [testCity] = new List<Restaurant> { testRestaurant }
            };

            Dictionary<string, List<Restaurant>> searchedRestaurants = testCatalog.SearchRestaurants(restaurants);

            if (searchedRestaurants.Count == 0)
            {
                Console.WriteLine("Search test 2 PASSED");
            }

            else if (searchedRestaurants.Count != 0)
            {
                Console.WriteLine("Search test 2 FAILED (not 1)");
            }
            else
            {
                Console.WriteLine("Search test 2 FAILED (unexpected error)");
            }
        }
    }
}