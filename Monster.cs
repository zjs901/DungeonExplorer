using System;

namespace DungeonExplorer
{
    // Use of abstract class
    public class Monster : Creature
    {
        private bool _isAlive;
        public bool HasDroppedItem { get; set; }
        private string _item;

        public Monster(Room monsterRoom, string name, int health, int maxDamage, int minDamage, bool isAlive, string item) 
            : base(monsterRoom, name, health, maxDamage, minDamage) 
        {

            IsAlive = isAlive;
            _item = item;
        }
        // Use of override
        public override void Attack(Creature target)
        {
            int monsterAttack = rand.Next(MinDamage, MaxDamage + 1);
            target.Health -= monsterAttack;
            Console.WriteLine($"{target.Name} hit back for {monsterAttack} damage.");

        }

        // Getters and setters, for the monsters, name, health, max damage, min damage, alive status and the item it drops
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
