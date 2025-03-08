using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DungeonExplorer
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
