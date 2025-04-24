// This file created by Emily 4/23
using DineAuto.Pages;
using DineAuto.Pages.CreateAccounts;
using DineAuto.Pages.UserDashboards.CustomerDashboard;
using System.Transactions;

namespace DineAuto.Testing
{
	public partial class Tests
	{
		// Test case 01a: Customer Registration. Unit testing -- only testing AddUser(). 
		// Input: a blank customer data table, and sample user to add.
		// Expected output: This test should be the only entry in the customer table once it's added.
		// Actual Output: matches expected outcome -- PASS.
		public static void TestCustomerRegistration_01a()
		{
			CreateCustomerAccountModel testCreateCustomer = new CreateCustomerAccountModel();
			string testName = "TestName";
			string testPW = "TestPW";

			Dictionary<string, string> usersTest = new Dictionary<string, string>();
			testCreateCustomer.users = usersTest;

			testCreateCustomer.AddUser(testName, testPW);

			if (usersTest.Count == 1 && usersTest.ContainsKey(testName) && BCrypt.Net.BCrypt.Verify(testPW, usersTest[testName]))
			{
				Console.WriteLine("Add Customer Test PASSED.");
				return;
			}
			else
			{
				Console.WriteLine("Add Customer Test FAILED.");
				return;
			}
		}

		// Test case 01b: Customer Registration. Unit Testinfg on AddUser().
		// Input: a customer table containing a match to the one getting added. 
		// Expected output: should fail to add a duplicate username regardless of pw.
		public static void TestCustomerRegistration_01b()
		{
			CreateCustomerAccountModel testCreateCustomer = new CreateCustomerAccountModel();
			string testName = "TestName";
			string testPW = "TestPW";

			Dictionary<string, string> usersTest = new Dictionary<string, string>();
			usersTest["TestName"] = testPW;

			testCreateCustomer.users = usersTest;
			testCreateCustomer.AddUser(testName, testPW); // should fail here

			if (usersTest.Count == 1 && testCreateCustomer.users[testName] == testPW)
			{
				Console.WriteLine("Add Customer Test with dupe PASSED.");
				return;
			}
			else
			{
				Console.WriteLine("Add Customer Test with dupe FAILED.");
				return;
			}
		}

	}
}