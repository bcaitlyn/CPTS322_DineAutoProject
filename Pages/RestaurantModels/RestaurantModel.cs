using System;

namespace DineAuto.Pages.RestaurantModels
{
    /// <summary>
    /// Represents a restaurant with its name and owner.
    /// </summary>
    public class RestaurantModel
    {
        public string RestaurantName { get; set; }   // Name of the restaurant
        public string OwnerUsername { get; set; }    // Username of the owner who registered the restaurant

        /// <summary>
        /// Constructor to initialize a restaurant.
        /// </summary>
        public RestaurantModel(string restaurantName, string ownerUsername)
        {
            RestaurantName = restaurantName;
            OwnerUsername = ownerUsername;
        }
    }
}