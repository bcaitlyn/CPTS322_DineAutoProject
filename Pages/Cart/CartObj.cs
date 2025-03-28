namespace DineAuto.Pages.Cart
{
    public class CartObj
    {
        private List<Item> items;
        private int size; 
        public CartObj() 
        {
            this.size = 0;
            this.items = new List<Item>();
        }
    }
}
