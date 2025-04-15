using System;

namespace DungeonExplorer.Items
{
    public class Potion : Item
    {
        public Potion(string name, string description, int health)
            :base(name, description)
        {
            HealthPoints = health;
        }
        
        public int HealthPoints
        {
            get;
            set;
        }

        public override void Use(Player player)
        {
            Console.WriteLine($"You consume a {ItemName}, for {HealthPoints} health points.");
        }
    }
}