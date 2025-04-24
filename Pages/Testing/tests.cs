// This file created by Emily 4/23
using DineAuto.Pages;
using DineAuto.Pages.CreateAccounts;
using Microsoft.Extensions.Logging;

namespace DineAuto.Testing
{
	public static class Tests
	{
		public static bool TestCustomerRegistration_01()
		{
			CreateCustomerAccountModel testCreateCustomer = new CreateCustomerAccountModel();
			testCreateCustomer.AddUser("TestName", "TestPW");


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
		public static int Main(ILogger logger)
		{
			logger.LogInformation("TESTING STARTED!");

			logger.LogInformation("DONE TESTING! Running app . . .");
			return 0;
		}
	}
}