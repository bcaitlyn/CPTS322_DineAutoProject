
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
        
        public string ItemImage { get; private set; }
        public PrintItem(string itemName, decimal itemPrice, string itemImage)
        {
            
            this.ItemName = itemName;
            this.ItemPrice = itemPrice;
            this.ItemImage = itemImage;
        }
        
    }
}