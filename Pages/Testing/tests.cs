// This file created by Emily 4/23
using DineAuto.Pages;
using DineAuto.Pages.CreateAccounts;
using Microsoft.Extensions.Logging;

namespace DineAuto.Testing
{
	
	public class Tests
	{
		private static readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Tests>();
		// Test case 01: Customer Registration. 
		// Input: a blank customer data table, and sample user to add.
		// Input: a customer table containing a match to the one getting added. 
		// Expected output: should fail to add a duplicate.
		public static bool TestCustomerRegistration_01a()
		{
			CreateCustomerAccountModel testCreateCustomer = new CreateCustomerAccountModel();
			testCreateCustomer.AddUser("TestName", "TestPW");


			return true;
		}
		public static bool TestCustomerRegistration_01b()
		{
			CreateCustomerAccountModel testCreateCustomer = new CreateCustomerAccountModel();
			Dictionary<string, string> usersTest = new Dictionary<string, string>();
			testCreateCustomer.users = usersTest;
			testCreateCustomer.AddUser("TestName1", "TestPW2");


			return true;
		}


		// add more test functions here:

		//public static bool TestCustomerLogin_01()
		//{
		//	// Your logic here
		//	return true;
		//}

		//public static bool TestCustomerLogin_01()
		//{
		//	// Your logic here
		//	return true;
		//}


		// and call your test here. This main() is called from Program.cs right before the app runs.
		public static int Main()
		{
			
			logger.LogInformation("TESTING STARTED!");

			logger.LogInformation("DONE TESTING! Running app . . .");
			return 0;
		}
	}
}