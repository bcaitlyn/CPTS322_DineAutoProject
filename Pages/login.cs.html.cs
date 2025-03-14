using DineAuto.Pages.LoginMethods;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Newtonsoft.Json;

public class LoginModel : PageModel
{
    [BindProperty]
    public string UserRole { get; set; }

    [BindProperty]
    public string Username { get; set; }

    [BindProperty]
    public string Password { get; set; }

    public string Message { get; set; }

    private Dictionary<string, string> users;

    public void OnPost()
    {
        if(this.UserRole == "Admin")
        {
            AdminLoginMethods adminLoginMethods = new AdminLoginMethods();
            this.users = adminLoginMethods.LoadUsers();
            if (this.VerifyLogin())
            {
                // Redirect to Admin Page (Can Create Owner Accounts)
                
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";

            }
        }
        if(this.UserRole == "Customer")
        {
            CustomerLoginMethods customerLoginMethods = new CustomerLoginMethods();
            this.users = customerLoginMethods.LoadUsers();
            if (this.VerifyLogin())
            {
                // Redirect to customer page
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";

            }
        }
        if(this.UserRole == "Owner")
        {
            OwnerLoginMethods ownerLoginMethods = new OwnerLoginMethods();
            this.users = ownerLoginMethods.LoadUsers();
            if (this.VerifyLogin())
            {
                // Redirect to Owner Page (Can Create Employee Accounts)
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";

            }

        }

        
        if(this.UserRole == "Employee")
        {
            EmployeeCustomerLoginMethods employeeCustomerLoginMethods = new EmployeeCustomerLoginMethods();
            this.users = employeeCustomerLoginMethods.LoadUsers();
            if (this.VerifyLogin())
            {
                // Redirect to Employee Page
            }
            else
            {
                this.Message = "Username or Password was Incorrect, Please Try Again!";

            }

        }
        
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
