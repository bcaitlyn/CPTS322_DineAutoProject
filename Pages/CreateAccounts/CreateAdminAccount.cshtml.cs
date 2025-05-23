using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.CreateAccounts
{
    public class CreateAdminAccountModel : CreateUser
    {
        /// <summary>
        /// Created by Kaden. Inherits from CreateUser class. Does everything that the base class does except we change parameters. See more info in code below.
        /// The reason why these classes exist that inherit is because we create different kinds of users owners, admins, employees, customers. So for each type we must store the relevant info in the correct location.
        /// </summary>
        public CreateAdminAccountModel()
        {
            this.FilePath = "Tables/admins.json";
            this.users = this.LoadUsers();

        }
    }
}
