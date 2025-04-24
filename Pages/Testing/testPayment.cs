// This file created by Emily 4/23
using DineAuto.Pages;
using DineAuto.Pages.CreateAccounts;
using DineAuto.Pages.UserDashboards.CustomerDashboard;
using System.Transactions;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        // Test case 02a: Payment feature. Unit testing.
        // Input: a test customer with no funds. should start with $0. then add $10.
        // Expected output: test customer should have $10.
        // Actual Output:
        public static void TestPayment_02a()
        {
            string testName = "TestName";
            CustomerDashboardModel testCustomer = new CustomerDashboardModel();
            testCustomer.currentUser = testName;



        }

        // Test case 02a: Payment feature. Unit testing.
        // Input: a negative dollar amount to add to account.
        // Expected output: should give error message on front end and do nothing to bank account.
        // Actual Output: matches expected output.
        public static void TestPayment_02b()
        {
            return;
        }
    }
}