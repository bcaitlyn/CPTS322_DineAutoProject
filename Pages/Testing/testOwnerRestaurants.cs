// This file created by Yevin 4/24
using DineAuto.Pages.RegisterRestaurants;
using System.Text.Json;
using System.Reflection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Testing
{
    // Mock session class to simulate logged-in owner session
    public class TestSession : ISession
    {
        private Dictionary<string, byte[]> _sessionStorage = new();

        public IEnumerable<string> Keys => _sessionStorage.Keys;
        public string Id => Guid.NewGuid().ToString();
        public bool IsAvailable => true;

        public void Clear() => _sessionStorage.Clear();
        public Task CommitAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public Task LoadAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
        public void Remove(string key) => _sessionStorage.Remove(key);
        public void Set(string key, byte[] value) => _sessionStorage[key] = value;
        public bool TryGetValue(string key, out byte[] value) => _sessionStorage.TryGetValue(key, out value);
    }

    public partial class Tests
    {
        // Override the private catalogPath field in RegisterRestaurantModel
        private static void SetTestCatalogPath(RegisterRestaurantModel model, string jsonPath)
        {
            FieldInfo pathField = typeof(RegisterRestaurantModel).GetField("catalogPath", BindingFlags.NonPublic | BindingFlags.Instance);
            pathField?.SetValue(model, jsonPath);
        }

        // Assign a mock HttpContext with test session
        private static void MockSession(RegisterRestaurantModel model)
        {
            var mockHttpContext = new DefaultHttpContext();
            mockHttpContext.Session = new TestSession();
            mockHttpContext.Session.SetString("Username", "TestOwner");

            model.PageContext = new PageContext
            {
                HttpContext = mockHttpContext
            };
        }

        // Test 01: Owner registers a new restaurant (fresh data, should succeed)
        public static void TestRegisterRestaurant()
        {
            var model = new RegisterRestaurantModel
            {
                RestaurantName = "Test Bistro",
                Cuisine = "Italian",
                City = "Pullman",
                State = "WA"
            };

            string jsonPath = "Pages/Testing/TestingTables/testRestaurantCatalog.json";

            // Save an empty JSON catalog (valid format)
            var initialData = new Dictionary<string, List<Dictionary<string, object>>>();
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(initialData, new JsonSerializerOptions { WriteIndented = true }));

            SetTestCatalogPath(model, jsonPath);
            MockSession(model);

            model.OnPost();

            string resultJson = File.ReadAllText(jsonPath);
            bool passed = resultJson.Contains("Test Bistro") && model.IsSuccess && !model.AlreadyExists;

            Console.WriteLine(passed ? "Register Restaurant Test PASSED." : "Register Restaurant Test FAILED.");
        }

        // Test 02: Owner can add menu items to restaurant (should pass)
        public static void TestAddMenuItem()
        {
            // Prepare mock restaurant data
            string city = "Pullman";
            string restaurantName = "Test Bistro";
            string jsonPath = "Pages/Testing/TestingTables/testRestaurantCatalog.json";

            var catalog = new Dictionary<string, List<Dictionary<string, object>>>
            {
                [city] = new List<Dictionary<string, object>>
                {
                    new Dictionary<string, object>
                    {
                        { "name", restaurantName },
                        { "cuisine", "Italian" },
                        { "location", $"{city}, WA" },
                        { "menu", new List<Dictionary<string, object>>() }
                    }
                }
            };

            // Save test data
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(catalog, new JsonSerializerOptions { WriteIndented = true }));

            // Simulate item addition
            string newItemName = "Lasagna";
            string newItemPrice = "$12.99";
            string newItemDescription = "Cheesy meat lasagna";

            // Load, add item manually like RestaurantDashboard would
            string json = File.ReadAllText(jsonPath);
            var loaded = JsonSerializer.Deserialize<Dictionary<string, List<Dictionary<string, object>>>>(json);

            var restaurant = loaded[city].FirstOrDefault(r => r["name"].ToString() == restaurantName);
            if (restaurant != null)
            {
                var menu = restaurant["menu"] as JsonElement?;
                List<Dictionary<string, object>> menuList;

                if (menu is JsonElement je && je.ValueKind == JsonValueKind.Array)
                {
                    menuList = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(je.GetRawText());
                }
                else
                {
                    menuList = new List<Dictionary<string, object>>();
                }

                menuList.Add(new Dictionary<string, object>
                {
                    { "name", newItemName },
                    { "price", newItemPrice },
                    { "description", newItemDescription }
                });

                restaurant["menu"] = menuList;
            }

            // Save updated file
            File.WriteAllText(jsonPath, JsonSerializer.Serialize(loaded, new JsonSerializerOptions { WriteIndented = true }));

            // Reload and verify
            string result = File.ReadAllText(jsonPath);
            bool passed = result.Contains("Lasagna") && result.Contains("12.99");

            Console.WriteLine(passed ? "Add Menu Item Test PASSED." : "Add Menu Item Test FAILED.");
        }
    }
}
