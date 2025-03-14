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
        internal Dictionary<string, string> users;
        [BindProperty]
        public string UserRole { get; }

        public CreateUser()
        {

        }
        internal abstract void SaveUsers();

        internal Dictionary<string, string> LoadUsers()
        {
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();
        }

        public void AddUser(string username, string pw)
        {
            if (!users.ContainsKey(username))
            {
                users[username] = BCrypt.Net.BCrypt.HashPassword(pw); ;
                SaveUsers();
            }
        }

        public bool UserExists(string username)
        {
            return users.ContainsKey(username);
        }


    }
}
