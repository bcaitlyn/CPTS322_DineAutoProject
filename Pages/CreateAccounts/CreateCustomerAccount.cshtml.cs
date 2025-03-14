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

       

    }
}