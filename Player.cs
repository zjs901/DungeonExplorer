using System.Collections.Generic;
using System;

namespace DungeonExplorer
{
    public class Player : Creature
    {
        protected List<string> inventory = new List<string>();

        public Player(Room safeRoom, string name, int health, int maxDamage, int minDamage) 
            : base(safeRoom, name, health, maxDamage, minDamage)
        {
            
        }

        public override void Attack(Creature target)
        {
            int playerAttack = rand.Next(MinDamage,MaxDamage);
            target.Health -= playerAttack;
            Console.WriteLine($"You hit {target.Name} for {playerAttack} damage.");
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
    }
}