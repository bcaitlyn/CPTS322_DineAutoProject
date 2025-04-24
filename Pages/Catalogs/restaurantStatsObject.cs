using System;
using System.Collections.Generic;



/*
 * Class: RestaurantStats
 * Description: The statistics related to all orders from a restaurant.
 */
namespace DineAuto.Pages.Catalogs
{
    public class RestaurantStat
    {
        public int TotalOrders { get; set; }
        public double Revenue { get; set; }
        public string RestaurantName { get; set; }


        public RestaurantStat(int totalOrders, double revenue, string name)
        {
            this.TotalOrders = totalOrders;
            this.Revenue = revenue;
            this.RestaurantName = name;
        }


    }
}