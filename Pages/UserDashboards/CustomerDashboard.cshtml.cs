using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // add this in order to use HttpContext.Session.set/get...(). can only use this in onpost() or OnGet()
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Xml;

// this file created by Emily 03/28:
// Payment feature note: at this time customers ONLY exist in customerFunds.json if they have at some point added funds.
// So to check if they have enough to pay for their order, you have to check if they're in the table first.

// Potential bug/vulnerability: negativev add balance input is only checked on the front end. THis page does not.
// fixed 4/24 by Emily
namespace DineAuto.Pages.UserDashboards.CustomerDashboard
{
    public class CustomerDashboardModel : PageModel
    {
        public string currentUser = string.Empty;

        [BindProperty]
        public decimal Amount { get; set; }

        // underlying data structure of all the json files are key-value dictionaries
        //4/24 made this public so I can access it when testing.
        public Dictionary<string, decimal> userFunds;

        public string FilePath = "Tables/customerFunds.json";

        public void OnGet()
        {
            // this remembers what their username is because this was set at login
            currentUser = HttpContext.Session.GetString("Username");
        }

        public decimal getBalance() // this shows up dashboard as Balance: $
        {
            userFunds = LoadUsers();
            if (userFunds.ContainsKey(currentUser))
            {
                // user already has some funds
                return userFunds[currentUser];
            }
            else
            {
                return 0;
            }
        }

        public decimal getBalance(string username) // this shows up dashboard as Balance: $
        {
            userFunds = LoadUsers();
            if (userFunds.ContainsKey(username))
            {
                // user already has some funds
                return userFunds[username];
            }
            else
            {
                return 0.0m;
            }
        }

        public void OnPostAddFunds() // add funds button is type post, so looks for a function called OnPostAddFunds().
        {
            currentUser = HttpContext.Session.GetString("Username");
            if (Amount > 0) // "Amount" is taken from name field on input on the post form.
            {
                // find the right user:
                userFunds = LoadUsers();
                modifyFunds(currentUser, Amount);
            }
        }

        public void modifyFunds(string username, decimal amount)
        {
            if (userFunds.ContainsKey(username) && amount > 0.0m) // second clause added 4/24
            {
                // user already has some funds
                userFunds[username] += amount;
                SaveUsers();
            }

            else if (amount <= 0.0m) // added 4/24
            {
                return;
            }
             else
            {
                // this is the first time user is adding funds
                userFunds[username] = amount;
                SaveUsers(); // takes current userFunds dictionary and prints it to customerFunds.json
            }
        }

        public void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(userFunds, Newtonsoft.Json.Formatting.Indented);
            System.IO.File.WriteAllText(FilePath, json);
        }

        public Dictionary<string, decimal> LoadUsers()
        {
            if (System.IO.File.Exists(FilePath))
            {
                string json = System.IO.File.ReadAllText(FilePath);
                return JsonConvert.DeserializeObject<Dictionary<string, decimal>>(json) ?? new Dictionary<string, decimal>();
            }
            return new Dictionary<string, decimal>();
        }

    }
}