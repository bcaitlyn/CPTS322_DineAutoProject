namespace DineAuto.Pages.Cart
{
    public class CartObj
    {
        public List<Item> items = new List<Item>();
        public CartObj() 
        {
            this.items = new List<Item>();
        }
        public void AddItem(Item item)
        {
            this.items.Add(item);
        }
        public void RemoveItem(Guid id)
        {
            for(int i = 0; i < this.items.Count; i++)
            {
                if (this.items[i].ItemID == id)
                {
                    this.items.RemoveAt(i);
                    break;
                }
            }
        }
        public int GetTotal()
        {
            int total = 0;
            for(int i = 0; i < this.items.Count; i++)
            {
                total += this.items[i].ItemPrice;
            }
            return total;
        }
    }
}
