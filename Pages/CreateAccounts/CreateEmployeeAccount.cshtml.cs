using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DineAuto.Pages.CreateAccounts
{
    public class CreateEmployeeAccountModel : CreateUser
    {
        public CreateEmployeeAccountModel()
        {
            this.FilePath = "Tables/employees.json";
            this.users = this.LoadUsers();

        }
    }
}
