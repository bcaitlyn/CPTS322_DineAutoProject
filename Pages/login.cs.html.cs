using DineAuto.Pages.LoginMethods;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

public class LoginModel : PageModel
{
    [BindProperty]
    private string UserRole { get; }

    [BindProperty]
    private string Username { get; }

    [BindProperty]
    private string Password { get; }

    private string Message { get; set; }



    public void OnPost()
    {
        if (this.UserRole == "Admin")
        {

        }
    }


}
