using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

/*
 * This class sets up the model for the restaurant catalog.
 */
public class RestaurantCatalogModel : PageModel
{

    public List<Restaurant> Restaurants { get; set; } = new();

    public void OnGet()
    {
        var filePath = Path.Combine(_environment.ContentRootPath, "Tables", "Menus", "restaurants.json");

        if (System.IO.File.Exists(filePath))
        {
            var RestaurantsJson = System.IO.File.ReadAllText(filePath);
            Restaurants = JsonSerializer.Deserialize<List<Restaurant>>(RestaurantsJson) ?? new List<Restuarant>();
        }
    }
}

public class Restaurant
}
    public string Name { get; set; }
}

