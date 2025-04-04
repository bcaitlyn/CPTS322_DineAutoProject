
/*
 * Class: PrintItem
 * Description: a restaurant item. Created specifically for printing restaurant menus
 * in the restaurant catalog.
 * 
 * Programmer: Caitlyn Boyd
 * Last Modified: 4/3/25
 */
namespace DineAuto.Pages.Catalogs
{
    public class PrintItem
    {
        public string ItemName { get; private set; }
        public decimal ItemPrice { get; private set; }
        
        public PrintItem(string itemName, decimal itemPrice)
        {
            
            this.ItemName = itemName;
            this.ItemPrice = itemPrice;
        }
        
    }
}