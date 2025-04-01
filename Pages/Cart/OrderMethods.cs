using Newtonsoft.Json;

namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Handles saving orders to orders.json.
    /// </summary>
    public class OrderMethods
    {
        private readonly string filePath = Path.Combine(Directory.GetCurrentDirectory(), "Tables", "orders.json");

        public Dictionary<string, List<OrderObj>> LoadOrders()
        {
            if (File.Exists(filePath))
            {
                string json = File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, List<OrderObj>>>(json) ?? new Dictionary<string, List<OrderObj>>();
            }
            return new Dictionary<string, List<OrderObj>>();
        }

        public void SaveOrders(Dictionary<string, List<OrderObj>> orders)
        {
            string json = JsonConvert.SerializeObject(orders, Formatting.Indented);
            File.WriteAllText(filePath, json);
        }
    }
}
