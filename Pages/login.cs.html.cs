using DineAuto.Pages.Cart;
using DineAuto.Pages.LoginMethods;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;
using starter_project.Pages;

/// <summary>
/// Login Methods worked on by Kaden
/// Essentially this deals with all login functionality and handles redirects.
/// </summary>
public class LoginModel : PageModel
{
    // Bind property tags signify any sort of data that is gathered from html files. So if a user types in a username, the bind property binds that value and stores it in the data member.
    [BindProperty]
    public string? UserRole { get; set; }

    [BindProperty]
    public string? Username { get; set; }

    [BindProperty]
    public string? Password { get; set; }

    public string? Message { get; set; }

    private Dictionary<string, string>? users;
    private Dictionary<string, CartObj>? usersCart;

    /// <summary>
    /// Kaden Worked on 3-25-25
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
                //Store user data in the session. This will allow users to stay logged in across pages.
                HttpContext.Session.SetString("UserRole", this.UserRole);


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
            CartMethods cartMethods = new CartMethods();
            this.users = customerLoginMethods.LoadUsers();
            this.usersCart = cartMethods.LoadUsersCart();
            if (this.VerifyLogin())
            {
                HttpContext.Session.SetString("UserRole", this.UserRole);
                HttpContext.Session.SetString("Username", this.Username!);
                if (!this.usersCart.ContainsKey(this.Username!))
                {
                    this.usersCart.Add(this.Username!, new CartObj());
                    cartMethods.AddUserCart(this.Username!);
                    cartMethods.SaveUsersCart(this.usersCart);
                }

                // Redirect to home page
                return RedirectToPage("/Index");
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";
                return Page();

            }
        }
        if (this.UserRole == "Owner")
        {
            OwnerLoginMethods ownerLoginMethods = new OwnerLoginMethods();
            this.users = ownerLoginMethods.LoadUsers();

            if (this.VerifyLogin())
            {
                HttpContext.Session.SetString("UserRole", this.UserRole);
                HttpContext.Session.SetString("Username", this.Username!);
                HttpContext.Session.SetString("Role", "Owner");

                // Redirect to Owner Dashboard
                return RedirectToPage("/UserDashboards/OwnerDashboard");
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
                HttpContext.Session.SetString("UserRole", this.UserRole);

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
    
    /// <summary>
    /// Method for verifying login
    /// </summary>
    /// <returns>Returns validation state, either login is true or false.</returns>
    public bool VerifyLogin()
    {
        // If our users dictionary doesnt contain the username return false.
        if (!(this.users!.ContainsKey(this.Username!)))
        {
            
            return false;

        }
        else
        {
            // Since the useranme exists, check if the password matches our database.
            if (BCrypt.Net.BCrypt.Verify(this.Password, this.users[this.Username!]))
            {
                return true;
            }
            else
            {

                return false;

            }
        }
    }

    public IActionResult OnPostLogout()
    {
        // Clear the session Data
        HttpContext.Session.Clear();



        // After logging out, redirect the user to the login page
        return RedirectToPage("/Index");
    }
}
