namespace DineAuto.Pages.Cart
{
    /// <summary>
    /// Item class which stores properties of an item
    /// </summary>
    public class Item
    {
        public Guid ItemID { get; private set; }
        public string ItemName { get; private set; }
        public int ItemPrice { get; private set; }
        public string RestaurantName { get; set; }

        public Item(string itemName, int itemPrice, Guid? itemID = null)
        {
            this.ItemID = itemID ?? Guid.NewGuid();
            this.ItemName = itemName;
            this.ItemPrice = itemPrice;
        }
    }
}
