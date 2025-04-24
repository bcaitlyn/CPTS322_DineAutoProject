//file created on 4/24/25

using DineAuto.Pages.Cart;
using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        /// <summary>
        /// Method taken from CartMethods but modified for testing files.
        /// </summary>
        public static Dictionary<string, CartObj> LoadUsersCart()
        {
            string filePath = "Pages/Testing/TestingTables/testCarts.json";
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, CartObj>>(json) ?? new Dictionary<string, CartObj>();
            }
            return new Dictionary<string, CartObj>();
        }

        /// <summary>
        /// Method taken from CartMethods but modified for testing files.
        /// </summary>
        public static void SaveUsersCart(Dictionary<string, CartObj> usersCart)
        {
            string filePath = "Pages/Testing/TestingTables/testCarts.json";
            string json = JsonConvert.SerializeObject(usersCart, Formatting.Indented);
            System.IO.File.WriteAllText(filePath, json);
        }

        /// <summary>
        /// Method taken from CartMethods but modified for testing files.
        /// </summary>
        public void AddUserCart(Dictionary<string, CartObj> usersCarts, string username)
        {
            usersCarts = LoadUsersCart();
            if (!usersCarts.ContainsKey(username))
            {
                usersCarts[username] = new CartObj();
            }
        }

        public static void TestAddItemToCart()
        {
            Item burger = new Item("Cheese Burger", 12, "Coug Eats", new Guid());
            Dictionary<string, CartObj> usersCarts = LoadUsersCart();
            CartObj userCart = usersCarts["TestName"];
            userCart.AddItem(burger);
            SaveUsersCart(usersCarts);

            string jsonFile = File.ReadAllText("Pages/Testing/TestingTables/testCarts.json");

            if(jsonFile.Contains("Cheese Burger") && jsonFile.Contains("Coug Eats"))
            {
                Console.WriteLine("Add Item To Cart Test PASSED");
                return;
            }
            else
            {
                Console.WriteLine("Add Item To Cart Test FAILED");
                return;
            }
        }

        public static void TestClearCart()
        {
            Dictionary<string, CartObj> usersCarts = LoadUsersCart();
            CartObj userCart = usersCarts["TestName"];
            userCart.items.Clear();
            SaveUsersCart(usersCarts);

            string jsonFile = File.ReadAllText("Pages/Testing/TestingTables/testCarts.json");

            if (!jsonFile.Contains("Cheese Burger") && !jsonFile.Contains("Coug Eats"))
            {
                Console.WriteLine("Clear Cart Test PASSED");
                return;
            }
            else
            {
                Console.WriteLine("Clear Cart Test FAILED");
                return;
            }

        }

    }
}
