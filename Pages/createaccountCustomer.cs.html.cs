using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using BCrypt.Net;

public class CreateAccountModel : PageModel
{
    private const string FilePath = "Tables/customers.json";
    private Dictionary<string, string> users;

    public CreateAccountModel()
    {
        users = LoadUsers();
    }

    private Dictionary<string, string> LoadUsers()
    {
        if (System.IO.File.Exists(FilePath))
        {
            string json = System.IO.File.ReadAllText(FilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
        }
        return new Dictionary<string, string>();
    }

    private void SaveUsers()
    {
        string json = JsonConvert.SerializeObject(users, Formatting.Indented);
        System.IO.File.WriteAllText(FilePath, json);
    }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    public void OnPost()
    {
        if (!UserExists(Username))
        {
            AddUser(Username, Password);
            Message = "Account successfully created";
        }
        else
        {
            Message = "Username already exists please try again";
        }
    }

    public void AddUser(string username, string pw)
    {
        if (!users.ContainsKey(username))
        {
            users[username] = BCrypt.Net.BCrypt.HashPassword(pw);;
            SaveUsers();
        }
    }

    public bool UserExists(string username)
    {
        return users.ContainsKey(username);
    }
}