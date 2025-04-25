using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DineAuto.Pages.Cart;

namespace DineAuto.Pages.OrderHistory
{
    public class OrderHistoryModel : PageModel
    {
        public List<OrderObj> curUserOrderHistory = new List<OrderObj>();
        public void OnGet()
        {
            OrderMethods orderMethods = new OrderMethods();
            Dictionary<string, List<OrderObj>> orders = orderMethods.LoadOrders();
            if (HttpContext.Session.GetString("Username") != null)
            {
                if (orders.ContainsKey(HttpContext.Session.GetString("Username")))
                {
                    this.curUserOrderHistory = orders[HttpContext.Session.GetString("Username")];
                }
            }
        }
    }
}
