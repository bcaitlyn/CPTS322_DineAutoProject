namespace DineAuto.Pages.RestaurantModels
{
    /// <summary>
    /// Represents a single menu item.
    /// </summary>
    public class MenuItem
    {
        public string Name { get; set; }            // Name of the item
        public decimal Price { get; set; }          // Price of the item
        public string Description { get; set; }     // Description of the item

        // Parameterless constructor required for JSON deserialization
        public MenuItem()
        {
        }

        /// <summary>
        /// Constructor to initialize a menu item.
        /// </summary>
        public MenuItem(string name, decimal price, string description)
        {
            Name = name;
            Price = price;
            Description = description;
        }
    }
}
