﻿namespace DungeonExplorer
{
    public class Monster
    {
        private string _name;
        private int _health;
        private int _maxDamage;
        private int _minDamage;
        private bool _isAlive;
        private string _item;

        public Monster(string name, int health, int maxDamage, int minDamage, bool isAlive, string item)
        {
            Name = name;
            Health = health;
            MaxDamage = maxDamage;
            MinDamage = minDamage;
            IsAlive = isAlive;
            _item = item;
        }

        // Getters and setters, for the monsters, name, health, max damage, min damage, alive status and the item it drops
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
        public bool IsAlive
        {
            get
            {
                return _isAlive;
            }
            set
            {
                _isAlive = value;
            }
        }

        public string Item
        {
            get
            {
                return _item;
            }
            set
            {
                _item = value;
            }
        }
    }
}
