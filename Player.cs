using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Player
    {
        private Room _currentRoom;
        private string _name;
        private int _health;
        private int _damage;
        private List<string> inventory = new List<string>();

        public Player(Room currentroom, string name, int health, int damage) 
        {
            CurrentRoom = currentroom;
            Name = name;
            Health = health;
            Damage = damage;

        }
        public void PickUpItem(string item)
        {
            inventory.Add(item);
        }
        public string InventoryContents()
        {
            return string.Join(", ", inventory);
        }

        public Room CurrentRoom { get; set; }

        public string Name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        public int Health
        {
            get
            {
                return _health;
            }
            set
            {
                _health = value;
            }
        }

        public int Damage
        {
            get
            {
                return _damage;
            }
            set
            {
                _damage = value;
            }
        }
    }
}