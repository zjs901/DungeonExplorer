using System;

namespace DungeonExplorer.Items
{
    public class Weapon : Item
    {
        public Weapon(string name, string description, int damage)
            :base(name, description)
        {
            Damage = damage;
        }
        
        public int Damage
        {
            get;
            set;
        }
        
        public override void Use(Player player)
        {
        }
    }
}