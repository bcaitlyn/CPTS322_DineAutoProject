using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Reflection;
using DineAuto.Pages.Cart;
using DineAuto.Pages.Catalogs;

namespace DineAuto.Testing
{
    public class RestaurantCatalogTest
    {
        public RestaurantCatalog testCatalog = new RestaurantCatalog();

        /*
         * Method: testLoadRestaurants
         * Description: Tests the LoadRestaurants method to the RestaurantCatalog.
         */
        public void testLoadRestaurants()
        {
            
            Dictionary<string, List<Restaurant>> result = testCatalog.LoadRestaurants();

            if (result != null)
            {
                Console.WriteLine("Load Restaurant Test: PASSED");
            }
            else
            {
                Console.WriteLine("Load Restaurant Test: FAILED");
            }
        }

        /*
         * Method: CalculateStatsTests()
         * Description: Tests the Caclulate Stats Method of the Restaurant Catalog.
         */
        public void CalculateStatsTests()
        {
            
            Dictionary<string, List<Restaurant>> test1 = testCatalog.LoadRestaurants();
            List<RestaurantStat> resultStats1 = testCatalog.CalculateStats(test1);

            if (resultStats1 != null)
            {
                Console.WriteLine("Calculate Stats Test: Passed");
            }
            else
            {
                Console.WriteLine("Calculate Stats Test: Failed");
            }

        }
        /*
         * Method: CalculateStatTests
         * Description: Tests the CalculateStat method of the Restaurant Catalog when the name
         * of the restuarant is not in the catalog and when it is.
         */
        public void CalculateStatTests()
        {
            // restaurant not in catalog
            string notInCatalogName = "wrong";
            string inCatalogName = "Tacoma BBQ";

            
            RestaurantStat result1 = testCatalog.CalculateStat(notInCatalogName);
            RestaurantStat result2 = testCatalog.CalculateStat(inCatalogName);

            if (result1 != null)
            {
                Console.WriteLine("Caclulate Stat of Restaurant Not in Catalog Test: Passed");
            }
            else
            {
                Console.WriteLine("Calculate Stat of Restaurant Not in Catalog Test: Failed");
            }

            if (result2 != null)
            {
                Console.WriteLine("Calculate Stat of Restaurant in Catalog Test: Passed");
            }
            else
            {
                Console.WriteLine("Caclulate Stat of Restaurant in Catalog Test: Failed");
            }
        }
        
    }



}


