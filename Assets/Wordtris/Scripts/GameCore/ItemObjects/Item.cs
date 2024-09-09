namespace Wordtris.GameCore.ItemObjects
{
    public abstract class Item
    {
        public string Name { get; private set; }
        public int Cost { get; private set; }

        protected Item(string name, int cost)
        {
            Name = name;
            Cost = cost;
        }

        // Each item has its unique effect.
        public abstract void UseItem();
    }
}

