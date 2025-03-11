using System;
using System.Diagnostics;
using System.Linq;
using System.Media;
using System.Reflection;
using System.Threading;

namespace DungeonExplorer
{
    internal class Game
    {
        Random rand = new Random();
        private Player player;
        public Room safeRoom { get; private set; }
        public Room monsterRoom { get; private set; }
        public Monster monster { get; private set; }

        public Game()
        {
            // Initialize the game with rooms and one player
            safeRoom = new Room("The Safe Room", "Take a seat, and relax", false, false, true);
            monsterRoom = new Room("The Monster Room", "A dark room, that could contain monsters", true, true, false);

            player = new Player(safeRoom, "", 100, 8);

            monster = new Monster("The Kernel", 50, 15, 10, true, "Diamond Key");

        }
        public void Start()
        {
            bool playing = true;
            
            // Combat(); - [testing purposes]

            Console.WriteLine("Welcome to the Dungeon!");

            // Checking name is valid
            while (true)
            {
                Console.Write("What is your name: ");
                player.Name = Console.ReadLine();

                if (player.Name.Any(char.IsDigit))
                    Console.WriteLine("Please use only characters, try again!");
                else if (player.Name.Length > 16)
                    Console.WriteLine("Your name must be less than 16 characters!");
                else if (player.Name == "")
                    Console.WriteLine("This is invalid. Please try again!");
                else if (player.Name.All(char.IsLetter))
                    break;
                else
                    Console.WriteLine("This is invalid. Please try again!");
            }

            while (playing)
            {
                Menu();
            }
        }

        // Quit game
        public void Quit()
        {
            Console.WriteLine("Quitting...");
            Environment.Exit(0);
        }

        // Display the menu for the user
        public void Menu()
        {
            Console.Clear();
            // Use of Room.GetDescription()
            Console.WriteLine($"{player.Name}, you are in the {player.CurrentRoom.Name}. {player.CurrentRoom.GetDescription()}.");
            Console.WriteLine("Actions");
            Console.WriteLine("1. View Inventory");
            Console.WriteLine("2. Show Stats");
            Console.WriteLine("3. Travel");
            Console.WriteLine("4. Quit"); Console.Write("> ");
            string menuInput = Console.ReadLine();

            if (menuInput == "1")
            {
                // View Inventory
                Console.Clear();
                Console.WriteLine($"Your Inventory: {player.InventoryContents()}\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            else if (menuInput == "2")
            {
                // Shows Stats
                Console.Clear();
                Console.WriteLine("Your Stats:");
                Console.WriteLine($"HEALTH: {player.Health} DAMAGE: {player.Damage}\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            else if (menuInput == "3")
                // Navigate Map
                Navigation();
            else if (menuInput == "4")
                // Quits the game
                Quit();
            else
            {
                Console.WriteLine("This is not a valid input. Try again!");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            Menu();
        }

        public void Navigation()
        {
            while (true)
            {
                // Displays where the player is, and where they can travel to
                Console.Clear();
                Console.WriteLine($"You are in {player.CurrentRoom.Name}");
                Console.WriteLine("Available destinations:");
                if (player.CurrentRoom.North == true)
                    Console.WriteLine("- North (The Safe Room)");
                if (player.CurrentRoom.South == true)
                    Console.WriteLine("- South (The Monster Room)");
                Console.WriteLine("'back' to go to the menu.");
                Console.Write("> ");
                string navigationInput = Console.ReadLine().ToLower();

                // Allows player to leave to the main menu
                if (navigationInput == "back")
                    Menu();

                // Quits the game
                if (navigationInput == "quit")
                    Quit();

                // Travels north if the user inputs north
                if (navigationInput == "north" || navigationInput == "n")
                {
                    if (player.CurrentRoom.North)
                    {
                        player.CurrentRoom = safeRoom;
                        Console.WriteLine($"Moving to {player.CurrentRoom.Name}...");
                        Thread.Sleep(1000);
                        break;
                    }
                }

                // Travels south if the user inputs south
                if (navigationInput == "south" || navigationInput == "s")
                {
                    if (player.CurrentRoom.South)
                    {
                        player.CurrentRoom = monsterRoom;
                        Console.WriteLine($"Moving to {player.CurrentRoom.Name}...");
                        Thread.Sleep(2000);
                        CheckMonsterRoom();
                        break;
                    }
                }

                // Error checking preventing the player from entering a room that does not exist
                else if (navigationInput == "west" || navigationInput == "w")
                {
                    Console.WriteLine("Invalid direction. Please try again!");
                    Console.ReadKey();
                }

                else if (navigationInput == "east" || navigationInput == "e")
                {
                    Console.WriteLine("Invalid direction. Please try again!");
                    Console.ReadKey();
                }

                else
                {
                    Console.WriteLine("Invalid input. Please try again!");
                    Console.ReadKey();
                }
            }
        }

        // Starts the combat system
        public void Combat()
        {
            while (player.Health > 0 && monster.Health > 0)
            {
                Console.Clear();
                Console.WriteLine($"An enemy has appeared! It's {monster.Name}.");
                Console.WriteLine($"HEALTH: {monster.Health} DAMAGE: {monster.MinDamage}-{monster.MaxDamage}");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Defend");
                Console.WriteLine("3. Inventory");
                Console.WriteLine("4. Run");
                Console.Write("> ");
                string combatInput = Console.ReadLine();

                // Attack the monster
                if (combatInput == "1")
                {
                    Console.Clear();
                    Console.WriteLine("Attacking...");

                    // Hit the monster
                    monster.Health -= player.Damage;
                    Console.WriteLine($"You hit {monster.Name} for {player.Damage} damage.");
                    Thread.Sleep(1500);

                    // Monster hits back
                    int monsterAttack = rand.Next(monster.MinDamage, monster.MaxDamage);
                    player.Health -= monsterAttack;

                    // Sets monster health to 0, if the monster is less than 0, after players attack
                    if (monster.Health < 0)
                        monster.Health = 0;

                    Console.WriteLine($"{monster.Name} hit back for {monsterAttack} damage.");
                    Console.WriteLine($"You have {player.Health} health\n{monster.Name} has {monster.Health} health.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadLine();

                    // Check player health is not below 0 after taking damage
                    // Debug.Assert(player.Health >= 0, $"Player's health is less than 0: {player.Health}");

                    // Check monster health is not below 0 after being attacked
                    // Debug.Assert(monster.Health >= 0, $"Monster's health is less than 0: {monster.Health}");

                }

                // Defend attack from the monster
                else if (combatInput == "2")
                {
                    Console.Clear();
                    Console.WriteLine("Defending...");
                    Thread.Sleep(1500);

                    int monsterAttack = rand.Next(monster.MinDamage, monster.MaxDamage);
                    int defendAttack = monsterAttack / 4;
                    player.Health -= defendAttack;
                    Console.WriteLine($"You defended the hit from {monster.Name}. They did {defendAttack} damage.");
                    Console.WriteLine($"You have {player.Health} health\n{monster.Name} has {monster.Health} health.");
                    Console.WriteLine("\nPress any key to continue...");
                    Console.ReadLine();

                }

                // View inventory
                else if (combatInput == "3")
                {
                    Console.Clear();
                    Console.WriteLine($"Inventory: {player.InventoryContents()}");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                }

                // Run from the monster
                else if (combatInput == "4")
                {
                    Console.Clear();
                    Console.WriteLine("You ran back to the safe room.");
                    player.CurrentRoom = safeRoom;
                    monster.Health = 50;
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                    Navigation();
                }
                else
                    Console.WriteLine("Invalid input, try again...");
            }

            // When you break out of the combat while loop, set the monster to dead
            //monster.Health = 0;
            monster.IsAlive = false;
            CheckMonsterDead();
        }

        // Checks if monster is alive or not, if so, start combat
        public void CheckMonsterRoom()
        {
            if (monster.IsAlive == true)
            {
                Console.Clear();
                Combat();
            }
        }

        // Checks if the monster is dead, so we can drop the item
        public void CheckMonsterDead()
        {
            // Check if the monster health is exactly 0 when dead. If not, there is an error message
            // Debug.Assert(monster.Health == 0, $"Monster health should be 0 when killed. Monster Health is: {monster.Health}");

            if (monster.IsAlive == false)
            {
                Console.WriteLine($"You killed {monster.Name}! They dropped an item: '{monster.Item}'.");
                // Use of Player.PickUpItem
                player.PickUpItem(monster.Item);
                Console.WriteLine($"+1 {monster.Item}");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
        }
    }
}