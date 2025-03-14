using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.CreateAccounts
{
    public class CreateAdminAccountModel : CreateUser
    {
        public CreateAdminAccountModel()
        {
            this.FilePath = "Tables/admins.json";
            this.users = this.LoadUsers();

        }
    }
}
