using DineAuto.Pages.Cart;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DineAuto.Pages.CreateAccounts
{
   
    public abstract class CreateUser : PageModel
    {

        [BindProperty]
        public string Username { get; set; }

        [BindProperty]
        public string Password { get; set; }

        public string Message { get; set; }

        internal string FilePath = string.Empty;
        internal string CartFilePath = string.Empty;
        internal Dictionary<string, string> users;
        internal Dictionary<string, CartObj> usersCart;
        [BindProperty]
        public string UserRole { get; }

        public void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(this.users, Formatting.Indented);
            System.IO.File.WriteAllText(FilePath, json);
        }

        internal void SaveUsersCart()
        {
            string json = JsonConvert.SerializeObject(this.usersCart, Formatting.Indented);
            System.IO.File.WriteAllText(this.CartFilePath, json);
        }
        internal Dictionary<string, string> LoadUsers()
        {
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();
        }

        internal Dictionary<string, CartObj> LoadUsersCarts()
        {
            if (System.IO.File.Exists(this.CartFilePath))
            {
                string json = System.IO.File.ReadAllText(this.CartFilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, CartObj>>(json) ?? new Dictionary<string, CartObj>();
            }
            return new Dictionary<string, CartObj>();
        }

        public void AddUser(string username, string pw)
        {
            if (!users.ContainsKey(username))
            {
                users[username] = BCrypt.Net.BCrypt.HashPassword(pw); ;
                SaveUsers();
            }
        }

        public void AddUsersCart(string username)
        {
            this.usersCart[username] = new CartObj();
        }

        public bool UserExists(string username)
        {
            return users.ContainsKey(username);
        }

        public void OnPost()
        {
            if (!this.UserExists(Username))
            {
                this.AddUser(Username, Password);
                this.AddUsersCart(Username);
                this.SaveUsersCart();
                this.Message = "Account successfully created";
            }
            else
            {
                this.Message = "Username already exists please try again";
            }
        }

    }
}
