namespace DineAuto.Testing
{
    using DineAuto.Pages.LoginMethods;
    using Newtonsoft.Json;

    public partial class Tests
    {
        /// <summary>
        /// Helper method for loading users, same as LoginMethods except the filePath has been swapped to a test json file.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<string, string> LoadUsers(string filePath)
        {
            if (System.IO.File.Exists(filePath))
            {
                string json = System.IO.File.ReadAllText(filePath);
                return JsonConvert.DeserializeObject<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
            }
            return new Dictionary<string, string>();

        }

        /// <summary>
        /// Slightly modified VerifyLogin Method, will be used to check if login is valid.
        /// </summary>
        /// <param name="users"></param>
        /// <param name="username"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static bool VerifyLogin(Dictionary<string, string> users, string username, string password)
        {
            // If our users dictionary doesnt contain the username return false.
            if (!(users.ContainsKey(username)))
            {

                return false;

            }
            else
            {
                // Since the username exists, check if the password matches our database.
                if (BCrypt.Net.BCrypt.Verify(password, users[username]))
                {
                    return true;
                }
                else
                {

                    return false;

                }
            }
        }

        public static void TestCustomerLogin()
        {
            string filePath = "Pages/Testing/TestingTables/testCustomers.json";
            Dictionary<string, string> users = LoadUsers(filePath);
            string username = "TestName";
            string password = "TestPW";
            if(VerifyLogin(users, username, password))
            {
                Console.WriteLine("Customer Login Test PASSED!");
                return;
            }
            else
            {
                Console.WriteLine("Customer Login Test FAILED!");
                return;
            }
        }

        public static void TestEmployeeLogin()
        {
            string filePath = "Pages/Testing/TestingTables/testEmployees.json";
            Dictionary<string, string> users = LoadUsers(filePath);
            string username = "EmployeeAccountTest";
            string password = "12345";
            if (VerifyLogin(users, username, password))
            {
                Console.WriteLine("Employee Login Test PASSED!");
                return;
            }
            else
            {
                Console.WriteLine("Employee Login Test FAILED!");
                return;
            }
        }

        public static void TestOwnerLogin()
        {
            string filePath = "Pages/Testing/TestingTables/testOwners.json";
            Dictionary<string, string> users = LoadUsers(filePath);
            string username = "OwnerAccountTest";
            string password = "12345";
            if (VerifyLogin(users, username, password))
            {
                Console.WriteLine("Owner Login Test PASSED!");
                return;
            }
            else
            {
                Console.WriteLine("Owner Login Test FAILED!");
                return;
            }
        }

        public static void TestAdminLogin()
        {
            string filePath = "Pages/Testing/TestingTables/testAdmins.json";
            Dictionary<string, string> users = LoadUsers(filePath);
            string username = "TestAdmin";
            string password = "12345";
            if (VerifyLogin(users, username, password))
            {
                Console.WriteLine("Admin Login Test PASSED!");
                return;
            }
            else
            {
                Console.WriteLine("Admin Login Test FAILED!");
                return;
            }
        }



    }
}
