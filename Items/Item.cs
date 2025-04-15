using System.ComponentModel;

namespace DungeonExplorer.Items
{
    public abstract class Item
    {
        public Item(string name, string description)
        {
            ItemName = name;
            ItemDescription = description;  
        }
        
        public string ItemName
        {
            get;
            set;
        }

        public string ItemDescription
        {
            get;
            set;
        }

        public abstract void Use(Player player);

    }
}