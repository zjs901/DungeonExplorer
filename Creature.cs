using System;
using System.Collections.Generic;

namespace DungeonExplorer
{
    public abstract class Creature
    {
        protected Random rand = new Random();
        private Room _currentRoom;
        private string _name;
        private int _health;
        private int _maxDamage;
        private int _minDamage;
        private List<string> inventory = new List<string>();

        public Creature(Room currentRoom, string name, int health, int maxDamage, int minDamage) 
        {
            CurrentRoom = currentRoom;
            Name = name;
            Health = health;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
        }
        // Has two different overrides depending on who is attacking
        public abstract void Attack(Creature target);
        
        // Getters and setters, for the players, current room, name, health, and damage
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