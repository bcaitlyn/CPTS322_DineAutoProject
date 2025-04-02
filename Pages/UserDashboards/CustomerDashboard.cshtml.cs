using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; // add this in order to use HttpContext.Session.set/get...(). can only use this in onpost() or OnGet()
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using System;
using System.Xml;

// this file created by Emily 03/28:
// Payment feature note: at this time customers ONLY exist in customerFunds.json if they have at some point added funds.
// So to check if they have enough to pay for their order, you have to check if they're in the table first.

// TO-DO: populate customerFunds.json with a 0 when they create account.

// resolved as of: ______

public class CustomerDashboardModel : PageModel
{
    public string currentUser = string.Empty;

    [BindProperty]
    public float Amount { get; set; }

    // underlying data structure of all the json files are key-value dictionaries
    private Dictionary<string, float> userFunds;

    public string FilePath = "Tables/customerFunds.json";

    public void OnGet()
    {
        // this remembers what their username is because this was set at login
        currentUser = HttpContext.Session.GetString("Username");
    }

    public float getBalance() // this shows up dashboard as Balance: $
    {
        userFunds = LoadUsers();
        Console.WriteLine(currentUser);
        Console.WriteLine(currentUser);
        if (userFunds.ContainsKey(currentUser))
        {
            // user already has some funds
            return userFunds[currentUser];
        }
        else
        {

            return 0f;
        }
    }

    public void OnPostAddFunds() // add funds button is type post, so looks for a function called OnPostAddFunds().
    {
        currentUser = HttpContext.Session.GetString("Username");
        if (Amount > 0) // "Amount" is taken from name field on input on the post form.
        {
            // find the right user:
            userFunds = LoadUsers();

            if (userFunds.ContainsKey(currentUser))
            {
                // user already has some funds
                userFunds[currentUser] += Amount;
                SaveUsers();
            }

            else
            {
                // this is the first time user is adding funds
                addUsertoFunds(Amount);
                SaveUsers(); // takes current userFunds dictionary and prints it to customerFunds.json
            }

        }
    }
    public void addUsertoFunds(float funds = 0.0f)
    {
        userFunds[currentUser] = funds;
        SaveUsers();
    }

    public void addUsertoFunds(string user, float funds = 0.0f)
    {
        userFunds[user] = funds;
        SaveUsers();
    }


    public void SaveUsers()
    {
        string json = JsonConvert.SerializeObject(userFunds, Newtonsoft.Json.Formatting.Indented);
        System.IO.File.WriteAllText(FilePath, json);
    }

    public Dictionary<string, float> LoadUsers()
    {
        if (System.IO.File.Exists(FilePath))
        {
            string json = System.IO.File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, float>>(json) ?? new Dictionary<string, float>();
        }
        return new Dictionary<string, float>();
    }
}
