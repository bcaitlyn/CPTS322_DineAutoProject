using DineAuto.Pages.LoginMethods;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using starter_project.Pages;

/// <summary>
/// Login Methods worked on by Emily and Kaden.
/// Essentially this deals with all login functionality and handles redirects.
/// </summary>
public class LoginModel : PageModel
{
    // Bind property tags signify any sort of data that is gathered from html files. So if a user types in a username, the bind property binds that value and stores it in the data member.
    [BindProperty]
    public string UserRole { get; set; }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    private Dictionary<string, string> users;

    /// <summary>
    /// Kaden Metzger Worked on 3-25-25
    /// This method executes after hitting the login button.
    /// It loads all users (depending on the type of user) and checks if the login credentials match.
    /// </summary>
    /// <returns>returns a webpage that redirects the user.</returns>
    public IActionResult OnPost()
    {
        if(this.UserRole == "Admin")
        {
            AdminLoginMethods adminLoginMethods = new AdminLoginMethods();
            this.users = adminLoginMethods.LoadUsers();
            if (this.VerifyLogin())
            {
                // Redirect to home page
                return RedirectToPage("/Index");
                
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";
                return Page();

            }
        }
        if(this.UserRole == "Customer")
        {
            CustomerLoginMethods customerLoginMethods = new CustomerLoginMethods();
            this.users = customerLoginMethods.LoadUsers();
            if (this.VerifyLogin())
            {
                // Redirect to home page
                return RedirectToPage("/Index");
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";
                return Page();

            }
        }
        if(this.UserRole == "Owner")
        {
            OwnerLoginMethods ownerLoginMethods = new OwnerLoginMethods();
            this.users = ownerLoginMethods.LoadUsers();

            // if valid login
            if (this.VerifyLogin())
            {
                // Redirect to Owner Page (Can Create Employee Accounts)
                return RedirectToPage("/Index");
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";
                return Page();

            }
            
        }

        if(this.UserRole == "Employee")
        {
            EmployeeCustomerLoginMethods employeeCustomerLoginMethods = new EmployeeCustomerLoginMethods();
            this.users = employeeCustomerLoginMethods.LoadUsers();

            // If valid login
            if (this.VerifyLogin())
            {
                // Redirect to home Page
                return RedirectToPage("/Index");
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";
                return Page();

            }

        }
        return Page();

    }
    
    public bool VerifyLogin()
    {
        
        if (!(this.users.ContainsKey(this.Username)))
        {
            
            return false;

        }
        else
        {
            // Successful login
            if (BCrypt.Net.BCrypt.Verify(this.Password, this.users[this.Username]))
            {
                return true;
            }
            else
            {

                return false;

            }
        }
    }
    

}
