// This file created by Emily 4/23
using DineAuto.Pages;
using DineAuto.Pages.CreateAccounts;
using DineAuto.Pages.UserDashboards.CustomerDashboard;
using Microsoft.Extensions.Logging;
using System.Transactions;

namespace DineAuto.Testing
{
    public partial class Tests
    {
        private static readonly ILogger logger = LoggerFactory.Create(builder => builder.AddConsole()).CreateLogger<Tests>();

        // and call your test here. This Main() is called from Program.cs right before the app runs.
        public static int Main()
        {
            logger.LogInformation("TESTING STARTED!");
            TestCustomerRegistration_01a();
            TestCustomerRegistration_01b();
            logger.LogInformation("DONE TESTING! Running app . . .");
            return 0;
        }
    }
}