using Newtonsoft.Json;
using System.Runtime.CompilerServices;

namespace DineAuto.Pages.Cart
{
    public class CartMethods
    {
        private string FilePath { get; set; }
        private Dictionary<string, CartObj> usersCarts;

        public Dictionary<string, CartObj> LoadUsersCart()
        {
            this.FilePath = "Tables/carts.json";
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, CartObj>>(json) ?? new Dictionary<string, CartObj>();
            }
            return new Dictionary<string, CartObj>();
        }

        public void SaveUsersCart(Dictionary<string, CartObj> usersCart)
        {
            this.FilePath = "Tables/carts.json";
            string json = JsonConvert.SerializeObject(usersCart, Formatting.Indented);
            System.IO.File.WriteAllText(this.FilePath, json);
        }

        public void AddUserCart(string username)
        {
            this.usersCarts = this.LoadUsersCart();
            if (!this.usersCarts.ContainsKey(username))
            {
                this.usersCarts[username] = new CartObj();
            }
        }
    }
}
