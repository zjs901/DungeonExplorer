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
        public Monster boss { get; private set; }
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
            // Call testing classes to begind testing
            TestPlayerAttack();
            TestPotionHealing();
            TestInventoryAdd();

            InitializeRooms();
            player = new Player(StartingRoom(), "", 100, 15, 10);

            // Monsters
            boss = new Monster(bossRoom, "The Kernel", 75, 15, 5, true, "Diamond Key");
            monster = new Monster(monsterRoom, "Goblin", 25, 10, 5, true, "Potion of Healing");

            // Items
            HealingPotion = new Potion("Potion of Healing", "Can be used to heal you", 25);
            StoneSword = new Weapon("Stone Sword", "Might sword made of stone", 25);
        }

        public void InitializeRooms()
        {
            // Initialize the game with rooms and one player
            _rooms = new List<Room>(); // Create lists of rooms
            entrance = new Room("Entrance", "A dark, candle lit room, with 3 doors to choose from", false);
            monsterRoom = new Room("Monster Room", "There are broken chains and the wall, as if someone has escaped", true);
            chestRoom = new Room("Chest Room", "A bright room, with display cabinets", false);
            bossRoom = new Room("Boss Room", "A circular room with large trees climbing up the walls", true);
            
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

        // Display the menu for the user and use error handling to control the input
        public void Menu()
        {
            Console.Clear();
            Console.WriteLine($"{player.Name}, you are in the {player.CurrentRoom.Name}. {player.CurrentRoom.GetDescription()}.");
            Console.WriteLine("Actions");
            Console.WriteLine("1. View Inventory");
            Console.WriteLine("2. Show Stats");
            Console.WriteLine("3. Travel");
            Console.WriteLine("4. Check Room");
            Console.WriteLine("5. Quit");

            Console.Write("> ");
            string menuInput = Console.ReadLine();

            // Input validation
            if (!int.TryParse(menuInput, out int choice) || choice < 1 || choice > 5)
            {
                Console.WriteLine("Invalid input! Please enter a number (1-5).");
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
                // Re-display menu
                Menu();
                return;
            }

            switch (choice)
            {
                case 1:
                    ViewInventory();
                    Console.ReadKey();
                    break;
                case 2:
                    Console.Clear();
                    Console.WriteLine($"HEALTH: {player.Health} DAMAGE: {player.MinDamage}-{player.MaxDamage}\n");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadLine();
                    break;
                case 3:
                    Navigation();
                    break;
                case 4:
                    CheckRoom(player.CurrentRoom);
                    break;
                case 5:
                    Quit();
                    break;
            }
            // Return to menu
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
                
                Console.Write("> ");
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
        public void Combat()
        {
            Monster currentMonster = (player.CurrentRoom == bossRoom) ? boss : monster;

            while (player.Health > 0 && currentMonster.Health > 0)
            {
                Console.Clear();
                Console.WriteLine($"An enemy has appeared! It's {currentMonster.Name}.");
                Console.WriteLine($"HEALTH: {currentMonster.Health} DAMAGE: {currentMonster.MinDamage}-{currentMonster.MaxDamage}");
                Console.WriteLine("What do you want to do?");
                Console.WriteLine("1. Attack");
                Console.WriteLine("2. Defend");
                Console.WriteLine("3. Inventory");
                Console.WriteLine("4. Run");
                Console.Write("> ");

                string combatInput = Console.ReadLine()?.Trim();

                // Error handling for combat input
                if (!int.TryParse(combatInput, out int combat) || combat < 1 || combat > 4)
                {
                    Console.WriteLine("Invalid input! Please enter a number (1-4).");
                    Console.WriteLine("Press any key to continue...");
                    Console.ReadKey();
                    // Re-display combat options
                    continue;
                }


                switch (combat)
                {
                    case 1:
                        // Attack
                        Console.Clear();
                        Console.WriteLine("Attacking...");
                        player.Attack(currentMonster);

                        if (currentMonster.Health <= 0)
                            currentMonster.Health = 0;

                        currentMonster.Attack(player);
                        player.Health = Math.Max(0, player.Health);

                        Console.WriteLine($"You have {player.Health} health\n{currentMonster.Name} has {currentMonster.Health} health.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadLine(); 
                        break;
                    case 2:
                        // Defend
                        Console.Clear();
                        Console.WriteLine("Defending...");

                        int monsterAttack = rand.Next(currentMonster.MinDamage, currentMonster.MaxDamage + 1);
                        int defendAttack = monsterAttack / 2;
                        player.Health -= defendAttack;
                        player.Health = Math.Max(0, player.Health);

                        Console.WriteLine($"You defended against {currentMonster.Name}'s attack, reducing damage from {monsterAttack} to {defendAttack}.");
                        Console.WriteLine($"You have {player.Health} health\n{currentMonster.Name} has {currentMonster.Health} health.");
                        Console.WriteLine("\nPress any key to continue...");
                        Console.ReadLine();
                        break;
                    case 3:
                        // View inventory
                        ViewInventory();
                        Console.ReadKey();
                        break;
                    case 4:
                        // Run away
                        Console.Clear();
                        Console.WriteLine("You ran!");
                        player.CurrentRoom = entrance;
                        currentMonster.Health = (currentMonster == boss) ? 75 : 25; // Reset health
                        Console.WriteLine("Press any key to continue...");
                        Console.ReadLine();
                        Navigation();
                        return;
                    default:
                        Console.WriteLine("Invalid input, try again...");
                        break;
                }
            }

            if (player.Health <= 0)
            {
                Console.WriteLine($"You died to the {currentMonster.Name}!");
                player.Health = 0;
                GameOver();
            }
            else if (currentMonster.Health <= 0)
            {
                Console.WriteLine($"You killed {currentMonster.Name}!");
                currentMonster.Health = 0;
                MonsterDeath(currentMonster); // Pass the monster to handle drops
            }
            currentMonster.IsAlive = false;
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

        // View inventory with LINQ filtering
        public void ViewInventory()
        {
            Console.Clear();
            Console.WriteLine("Inventory:");

            var weapons = player.inventory.Where(item => item.Contains("Sword")).ToList();

            // Lists any weapons in the inventory
            if (weapons.Any())
            {
                Console.WriteLine("\nWeapons:");
                foreach (var weapon in weapons)
                {
                    Console.WriteLine($"- {weapon}");
                }
            }

            Console.WriteLine(player.InventoryContents());

            if (player.inventory.Contains("Potion of Healing"))
            {
                Console.WriteLine(" - Use Potion of Healing? (yes/no)");
                var usePotion = Console.ReadLine();

                if (usePotion == "yes")
                {
                    HealingPotion.Use(player);
                    player.inventory.Remove("Potion of Healing");
                    Console.WriteLine("You used the potion and healed!");
                }
                Console.WriteLine("Press any key to continue...");
                Console.ReadKey();
            }
            Console.ReadKey();
        }

        // When monster dies
        public void MonsterDeath(Monster defeatedMonster)
        {
            if (!defeatedMonster.HasDroppedItem)
            {
                Console.WriteLine($"They dropped an item: '{defeatedMonster.Item}'.");
                player.PickUpItem(defeatedMonster.Item);
                Console.WriteLine($"+1 {defeatedMonster.Item}");
                defeatedMonster.HasDroppedItem = true;
            }
            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey();
        }

        // TESTING CLASSES
        public static void TestPlayerAttack()
        {
            // Test the Player's attack method
            Console.WriteLine("=== Testing Player Attack ===");
            Player testPlayer = new Player(null, "Tester", 100, 10, 5);
            Monster testMonster = new Monster(null, "Dummy", 50, 5, 0, true, "");

            // Simulates an attack
            int initialHealth = testMonster.Health;
            testPlayer.Attack(testMonster);

            // Check if the monster's health has decreased
            Console.WriteLine($"Monster health before: {initialHealth}, after: {testMonster.Health}");
            Debug.Assert(testMonster.Health < initialHealth, "Attack should reduce monster health.");
            Console.WriteLine("Test passed!\n");
        }
        public static void TestPotionHealing()
        {
            // Test the Potion's healing effect
            Console.WriteLine("=== Testing Potion Healing ===");
            Player testPlayer = new Player(null, "Tester", 50, 10, 5);
            Potion testPotion = new Potion("Healing Potion", "Heals 20 HP", 20);

            // Simulates using a potion
            testPlayer.Health = 30;
            int healthBefore = testPlayer.Health;
            testPotion.Use(testPlayer);

            // Check if the player's health has increased
            Console.WriteLine($"Health before: {healthBefore}, after: {testPlayer.Health}");
            Debug.Assert(testPlayer.Health == 50, "Potion should heal to full.");
            Console.WriteLine("Test passed!\n");
        }
        public static void TestInventoryAdd()
        {
            // Test adding an item to the inventory
            Console.WriteLine("=== Testing Inventory ===");
            Player testPlayer = new Player(null, "Tester", 100, 10, 5);
            testPlayer.PickUpItem("Sword");

            // Check if the item is in the inventory
            Console.WriteLine($"Inventory contains Sword: {testPlayer.inventory.Contains("Sword")}");
            Debug.Assert(testPlayer.inventory.Contains("Sword"), "Sword should be in inventory.");
            Console.WriteLine("Test passed!\n");
        }
    }
}