using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using BCrypt.Net;
using DineAuto.Pages.CreateAccounts;

namespace DineAuto.Pages.CreateAccounts
{
    public class CreateCustomerAccountModel : CreateUser
    {
        public CreateCustomerAccountModel()
        {
            this.FilePath = "Tables/customers.json";
            this.users = this.LoadUsers();

        }

        internal override void SaveUsers()
        {
            string json = JsonConvert.SerializeObject(this.users, Formatting.Indented);
            System.IO.File.WriteAllText(FilePath, json);
        }



        public void OnPost()
        {
            if (!this.UserExists(Username))
            {
                this.AddUser(Username, Password);
                this.Message = "Account successfully created";
            }
            else
            {
                this.Message = "Username already exists please try again";
            }
        }

    }
}