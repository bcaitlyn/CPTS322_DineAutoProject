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
    /// <summary>
    /// Created by Kaden. Inherits from CreateUser class. Does everything that the base class does except we change parameters. See more info in code below.
    /// The reason why these classes exist that inherit is because we create different kinds of users owners, admins, employees, customers. So for each type we must store the relevant info in the correct location.
    /// </summary>
    public class CreateCustomerAccountModel : CreateUser
    {
        public CreateCustomerAccountModel()
        {
            this.FilePath = "Tables/customers.json";
            this.users = this.LoadUsers();

        }

       

    }
}