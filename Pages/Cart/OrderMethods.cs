using Newtonsoft.Json;

namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Handles saving and loading of customer orders to/from orders.json.
    /// </summary>
    public class OrderMethods
    {
        // Path to the orders.json file
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "orders.json");

        /// <summary>
        /// Loads all orders from orders.json file.
        /// </summary>
        public Dictionary<string, List<OrderObj>> LoadOrders()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, List<OrderObj>>>(json) ?? new Dictionary<string, List<OrderObj>>();
            }
            return new Dictionary<string, List<OrderObj>>();
        }

        /// <summary>
        /// Saves the provided orders dictionary to orders.json.
        /// </summary>
        public void SaveOrders(Dictionary<string, List<OrderObj>> orders)
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
