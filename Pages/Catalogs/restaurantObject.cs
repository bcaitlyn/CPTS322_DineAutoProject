using System;
using System.Collections.Generic;
using DineAuto.Pages.Cart;
using DineAuto.Pages.Catalogs;

/*
 * Class: Restaurant
 * Description: Encapsulates all the data related to a restaurant on DineAuto.
 * 
 * Programmer: Caitlyn Boyd
 * Last Modified: 4/3/25
 */

 // yevin 04/05 : Addition of OwnerUsername to store owner that created restaurant
 // Caitlyn 04/24: Added total orders
namespace DineAuto.Pages.Catalogs
{
	public class Restaurant
	{
		public string Name { get; set; }
		public string Cuisine { get; set; }
		public string Location { get; set; }
		public string OwnerUsername {get; set;}
		// CB: Added totalOrders and intTotalOrders 4/24/25
		

		public List<PrintItem> Menu;
        public int TotalOrders { get; set; }

        public Restaurant(string name, string cuisine, string location, List<PrintItem> menu, string ownerUsername, int totalOrders)
		{
			this.Name = name;
			this.Cuisine = cuisine;
			this.Location = location;
			this.Menu = menu;
			this.OwnerUsername = ownerUsername;
		}
	}
}