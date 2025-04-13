using System.Collections.Generic;
using System;

namespace DungeonExplorer
{
    // Use of abstract class
    public class Player : Creature
    {
        public List<string> inventory = new List<string>();

        public Player(Room safeRoom, string name, int health, int maxDamage, int minDamage) 
            : base(safeRoom, name, health, maxDamage, minDamage)
        {
            
        }
        // Use of override
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