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
            // Customer Registration Tests
            TestCustomerRegistration_01a();
            TestCustomerRegistration_01b();

            // Owner Restaurant Tests
            Tests.TestRegisterRestaurant();
            Tests.TestAddMenuItem();

            // Test Customer Orders (Itesm already in cart)
            Tests.TestPlaceValidOrder();
            Tests.TestPlaceOrderMultipleRestaurants();
            Tests.TestPlaceOrderEmptyCart();

            // Test Review system for restaurants and items
            Tests.TestLeaveRestaurantReview();
            Tests.TestLeaveItemReview();
            Tests.TestOwnerReplyToReview();

            //Tests for Payment
            Tests.TestPayment_02a();
            Tests.TestPayment_02b();

            //Tests for Search
            Tests.TestSearch_03a();
            Tests.TestSearch_03b();

            logger.LogInformation("DONE TESTING! Running app . . .");
            return 0;
        }
    }
}