// This file created by Emily 4/23
using DineAuto.Pages;
using DineAuto.Pages.CreateAccounts;
using DineAuto.Pages.UserDashboards.CustomerDashboard;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Transactions;
using static System.Net.Mime.MediaTypeNames;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        // Test case 02a: Payment feature. Unit testing getBalance.
        // Input: a test customer, then add $10 to their account
        // Expected output: test customer should have $10.
        // Actual Output: matches expected output.
        public static void TestPayment_02a()
        {
            try
            {
                string testName = "TestName1";
                CustomerDashboardModel testCustomer = new CustomerDashboardModel();
                testCustomer.currentUser = testName;
                testCustomer.FilePath = "Pages/Testing/TestingTables/testPayment.json";

                decimal initialBalance = testCustomer.getBalance();

                testCustomer.modifyFunds(testName, 10);

                decimal postBalance = testCustomer.getBalance();

                if (postBalance == initialBalance + 10.00m)
                {
                    Console.WriteLine("Payment test 1 PASSED.");
                }
                else
                {
                    Console.WriteLine("Payment test 1 FAILED.");
                }

                
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestPayment_02a() crashed: {ex.Message}");
            }
        }

        // Test case 02a: Payment feature. Unit testing.
        // Input: a negative dollar amount to add to account.
        // Expected output: should give error message on front end and do nothing to bank account.
        // Actual Output: matches expected output.

        // Potential bug/vulnerability: negativev add balance input is only checked on the front end.THis page does not.
        // fixed 4/24 by Emily
        public static void TestPayment_02b()
        {
            try
            {
                string testName = "TestName2";
                CustomerDashboardModel testCustomer = new CustomerDashboardModel();
                testCustomer.currentUser = testName;
                testCustomer.FilePath = "Pages/Testing/TestingTables/testPayment.json";

                decimal initialBalance = testCustomer.getBalance();
                //Console.WriteLine("Initial balance: " + initialBalance);

                testCustomer.modifyFunds(testName, -10);

                decimal postBalance = testCustomer.getBalance();
                if (initialBalance == postBalance)
                {
                    Console.WriteLine("Payment test 2 PASSED.");
                }
                else if (postBalance == initialBalance-10.00m)
                {
                    Console.WriteLine("Payment test 2 FAILED.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"TestPayment_02b() crashed: {ex.Message}");
            }
        }
    }
}