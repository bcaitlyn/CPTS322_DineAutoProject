namespace DineAuto.Pages.Cart
{
    public class CartObj
    {
        public List<Item> items = new List<Item>();
        private int size; 
        public CartObj() 
        {
            this.size = 0;
        }
        public void AddItem(Item item)
        {
            this.items.Add(item);
        }
    }
}
