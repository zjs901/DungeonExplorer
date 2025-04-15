using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using DungeonExplorer.Items;


namespace DungeonExplorer
{
    internal class Game
    {
        Random rand = new Random();
        private Player player;
        public Monster monster { get; private set; }
        // Rooms
        public Room entrance { get; private set; }
        public Room monsterRoom { get; private set; }
        public Room chestRoom { get; private set; }
        public Room bossRoom { get; private set; }
        
        public Potion HealingPotion { get; private set; }
        public Weapon StoneSword { get; private set; }

        
        private List<Room> _rooms;
        private Room _entrance;


        public Game()
        {
            InitializeRooms();
            player = new Player(StartingRoom(), "", 100, 15, 10);
            monster = new Monster(monsterRoom, "The Kernel", 50, 15, 5, true, "Diamond Key");
            
            // Items
            HealingPotion = new Potion("Potion of Healing", "Can be used to heal you", 25);
            StoneSword = new Weapon("Stone Sword", "Might sword made of stone", 25);
        }

        public void InitializeRooms()
        {
            // Initialize the game with rooms and one player
            _rooms = new List<Room>(); // Create lists of rooms
            entrance = new Room("Entrance", "PLACEHOLDER", false);
            monsterRoom = new Room("Monster Room", "PLACEHOLDER", false);
            chestRoom = new Room("Chest Room", "PLACEHOLDER", false);
            bossRoom = new Room("Boss Room", "PLACEHOLDER", true);
            
            // Connecting Rooms
            entrance.East = chestRoom;
            entrance.West = monsterRoom;
            entrance.South = bossRoom;
            
            monsterRoom.East = entrance;

            chestRoom.West = entrance;
            
            bossRoom.North = entrance;
            
            _rooms.Add(entrance);
            _rooms.Add(monsterRoom);
            _rooms.Add(chestRoom);
            _rooms.Add(bossRoom);
            
            _entrance = entrance;
        }

        public Room StartingRoom()
        {
            return _entrance;
        }
        
        public void Start()
        {
            bool playing = true;
            
            // Add potion to inventory
            //player.PickUpItem(HealingPotion.ItemName);
            
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

            player.CurrentRoom = StartingRoom();
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
            Console.WriteLine("4. Check Room");
            Console.WriteLine("5. Quit"); Console.Write("> ");
            string menuInput = Console.ReadLine();

            if (menuInput == "1")
            {
                // View Inventory
                ViewInventory();
            }
            else if (menuInput == "2")
            {
                // Shows Stats
                Console.Clear();
                Console.WriteLine("Your Stats:");
                Console.WriteLine($"HEALTH: {player.Health} DAMAGE: {player.MinDamage}-{player.MaxDamage}\n");
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
            }
            else if (menuInput == "3")
                // Navigate Map
                Navigation();
            else if (menuInput == "4")
                // Checks room for items etc...
                CheckRoom(player.CurrentRoom);
            else if (menuInput == "5")
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
                Console.Clear();
                Console.WriteLine($"You are in the {player.CurrentRoom.Name}.");

                if (player.CurrentRoom.Creature)
                {
                    Combat();
                }
                Console.Clear();
                Console.WriteLine("Travel:");
                if (player.CurrentRoom.North != null) Console.WriteLine($"- North to {player.CurrentRoom.North.Name}");
                if (player.CurrentRoom.South != null) Console.WriteLine($"- South to {player.CurrentRoom.South.Name}");
                if (player.CurrentRoom.East != null) Console.WriteLine($"- East to {player.CurrentRoom.East.Name}");
                if (player.CurrentRoom.West != null) Console.WriteLine($"- West to {player.CurrentRoom.West.Name}");
                
                Console.Write(">");
                string input = Console.ReadLine().ToLower();
                
                // Allows player to leave to the main menu
                if (input == "back")
                    Menu();

                // Quits the game
                if (input == "quit")
                    Quit();

                Room nextRoom = null;
                switch (input)
                {
                    case "north":
                        nextRoom = player.CurrentRoom.North;
                        break;
                    case "south":
                        nextRoom = player.CurrentRoom.South;
                        break;
                    case "east":
                        nextRoom = player.CurrentRoom.East;
                        break;
                    case "west":
                        nextRoom = player.CurrentRoom.West;
                        break;
                    default:
                        Console.WriteLine("This is not a valid input. Try again!");
                        break;
                }
                if (nextRoom == null)
                    Console.WriteLine("Cannot go that way!");
                else
                {
                    player.CurrentRoom = nextRoom;
                }
            }
        }

        // Starts the combat system
        public void Combat() {
            
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
            
            // Added null check
            string combatInput = Console.ReadLine()?.Trim();

            // Attack the monster
            if (combatInput == "1")
            {
                Console.Clear();
                Console.WriteLine("Attacking...");

                // Player attacks first
                player.Attack(monster);

                // Check if monster is dead before it can counterattack
                if (monster.Health <= 0)
                {
                    monster.Health = 0;
                }

                // Monster counterattacks
                monster.Attack(player);
                
                // Ensure health doesn't go below 0
                player.Health = Math.Max(0, player.Health);

                Console.WriteLine($"You have {player.Health} health\n{monster.Name} has {monster.Health} health.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadLine();
            }
            // Defend attack from the monster
            else if (combatInput == "2")
            {
                Console.Clear();
                Console.WriteLine("Defending...");

                int monsterAttack = rand.Next(monster.MinDamage, monster.MaxDamage + 1);
                int defendAttack = monsterAttack / 2;
                player.Health -= defendAttack;
                
                // Ensure health doesn't go below 0
                player.Health = Math.Max(0, player.Health);
                
                Console.WriteLine($"You defended against {monster.Name}'s attack, reducing damage from {monsterAttack} to {defendAttack}.");
                Console.WriteLine($"You have {player.Health} health\n{monster.Name} has {monster.Health} health.");
                Console.WriteLine("\nPress any key to continue...");
                Console.ReadLine();
            }
            // View inventory
            else if (combatInput == "3")
            {
                ViewInventory();
            }
            // Run from the monster
            else if (combatInput == "4")
            {
                Console.Clear();
                
                Console.WriteLine("You ran!");
                player.CurrentRoom = entrance;
                monster.Health = 50;
                Console.WriteLine("Press any key to continue...");
                Console.ReadLine();
                Navigation();
                return;
            }
            else
            {
                Console.WriteLine("Invalid input, try again...");
                Console.ReadLine();
            }
        }

        // Combat results
        if (player.Health <= 0)
        {
            Console.WriteLine($"You died to the {monster.Name}!");
            player.Health = 0;
            GameOver();
        }
        else if (monster.Health <= 0)
        {
            Console.WriteLine($"You killed {monster.Name}!");
            monster.Health = 0;
            MonsterDeath();
        }
        monster.IsAlive = false;
        player.CurrentRoom.Creature = false;
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
        public void MonsterDeath()
        {
            // Check if the monster health is exactly 0 when dead. If not, there is an error message
            // Debug.Assert(monster.Health == 0, $"Monster health should be 0 when killed. Monster Health is: {monster.Health}");
            
            if (!monster.HasDroppedItem)
            {
                Console.WriteLine($"They dropped an item: '{monster.Item}'.");
                player.PickUpItem(monster.Item);
                Console.WriteLine($"+1 {monster.Item}");
                monster.HasDroppedItem = true;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        public void GameOver()
        {
            Console.Clear();
            Console.WriteLine("Game Over! You lose...");
            Environment.Exit(0);
        }

        public void CheckRoom(Room room)
        {
            if (player.CurrentRoom == entrance)
            {
                Console.Clear();
                Console.WriteLine("There is a Wooden Chest. You need a Diamond Key to open it");
                if (player.inventory.Contains("Diamond Key"))
                {
                    Console.WriteLine("You can open the chest!");
                    Open();
                }
                else
                    Console.WriteLine("You can't open the chest!");
                Console.ReadKey();
            }
            else
            {
                Console.Clear();
                Console.WriteLine("There is nothing in this room.");
                Console.ReadKey();
            }
        }

        public void Open()
        {
            Console.WriteLine("Opening chest...");
            
        }

        public void ViewInventory()
        {
            Console.Clear();
            Console.WriteLine($"Inventory: {player.InventoryContents()}");
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }
    }
}