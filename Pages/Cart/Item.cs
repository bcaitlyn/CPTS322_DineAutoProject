namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Item class which stores properties of an item
    /// </summary>
    public class Item
    {
        // uint is unsigned it, meaning non negative int
        public uint ItemID { get; private set; }
        public string ItemName { get; private set; }
        public uint ItemPrice { get; private set; }

        public Item(uint itemID, string itemName, uint itemPrice)
        {
            this.ItemID = itemID;
            this.ItemName = itemName;
            this.ItemPrice = itemPrice;
        }
    }
}
