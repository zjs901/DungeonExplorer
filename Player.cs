using System.Collections.Generic;

namespace DungeonExplorer
{
    public class Player
    {
        private Room _currentRoom;
        private string _name;
        private int _health;
        private int _maxDamage;
        private int _minDamage;        
        private List<string> inventory = new List<string>();

        public Player(Room currentroom, string name, int health, int maxDamage, int minDamage) 
        {
            CurrentRoom = currentroom;
            Name = name;
            Health = health;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
        }

        // Getters and setters, for the players, current room, name, health, and damage
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

        public int MaxDamage
        {
            get
            {
                return _maxDamage;
            }
            set
            {
                _maxDamage = value;
            }
        }
        public int MinDamage
        {
            get
            {
                return _minDamage;
            }
            set
            {
                _minDamage = value;
            }
        }
    }
}