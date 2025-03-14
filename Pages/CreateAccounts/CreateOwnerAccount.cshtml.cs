using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

namespace DineAuto.Pages.CreateAccounts
{
    public class CreateOwnerAccountModel : CreateUser
    {
        public CreateOwnerAccountModel() 
        {
            this.FilePath = "Tables/owners.json";
            this.users = this.LoadUsers();
        }

       



    }
}
