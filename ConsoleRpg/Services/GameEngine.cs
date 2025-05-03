using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Inventory;

namespace ConsoleRpg.Services
{
    public class GameEngine
    {
        private readonly IInventoryService _inventoryService;

        public GameEngine(IInventoryService inventoryService)
        {
            _inventoryService = inventoryService;
        }

        public void Run()
        {
            var inventory = new List<InventoryItem>
            {
                new InventoryItem { Name = "Sword", Type = "Weapon", AttackValue = 50, DefenseValue = 10 },
                new InventoryItem { Name = "Shield", Type = "Armor", AttackValue = 10, DefenseValue = 50 },
                new InventoryItem { Name = "Bow", Type = "Weapon", AttackValue = 40, DefenseValue = 5 },
                new InventoryItem { Name = "Helmet", Type = "Armor", AttackValue = 5, DefenseValue = 30 }
            };

            MainMenu(inventory);
        }

        private void MainMenu(IEnumerable<InventoryItem> inventory)
        {
            while (true)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Search for an item by name");
                Console.WriteLine("2. List items by type");
                Console.WriteLine("3. Sort items");
                Console.WriteLine("4. Equip an item");
                Console.WriteLine("5. Use an item");
                Console.WriteLine("6. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        SearchItemByName(inventory);
                        break;

                    case "2":
                        ListItemsByType(inventory);
                        break;

                    case "3":
                        SortSubMenu(inventory);
                        break;

                    case "4":
                        EquipItemMenu(inventory);
                        break;

                    case "5":
                        UseItemMenu(inventory);
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private void SearchItemByName(IEnumerable<InventoryItem> inventory)
        {
            Console.Write("Enter item name to search: ");
            var name = Console.ReadLine();
            var searchResults = _inventoryService.SearchByName(inventory, name);
            foreach (var item in searchResults)
            {
                Console.WriteLine($"Name: {item.Name}, Type: {item.Type}, Attack: {item.AttackValue}, Defense: {item.DefenseValue}");
            }
        }

        private void ListItemsByType(IEnumerable<InventoryItem> inventory)
        {
            var groupedItems = _inventoryService.GroupByType(inventory);
            foreach (var group in groupedItems)
            {
                Console.WriteLine($"Type: {group.Key}");
                foreach (var item in group)
                {
                    Console.WriteLine($"  Name: {item.Name}, Attack: {item.AttackValue}, Defense: {item.DefenseValue}");
                }
            }
        }

        private void SortSubMenu(IEnumerable<InventoryItem> inventory)
        {
            Console.WriteLine("Sort Menu:");
            Console.WriteLine("1. Sort by Name");
            Console.WriteLine("2. Sort by Attack Value");
            Console.WriteLine("3. Sort by Defense Value");
            Console.Write("Select a sorting option: ");
            var sortChoice = Console.ReadLine();

            IEnumerable<InventoryItem> sortedItems = sortChoice switch
            {
                "1" => _inventoryService.SortByName(inventory),
                "2" => _inventoryService.SortByAttackValue(inventory),
                "3" => _inventoryService.SortByDefenseValue(inventory),
                _ => null
            };

            if (sortedItems == null)
            {
                Console.WriteLine("Invalid option. Returning to main menu.");
                return;
            }

            foreach (var item in sortedItems)
            {
                Console.WriteLine($"Name: {item.Name}, Type: {item.Type}, Attack: {item.AttackValue}, Defense: {item.DefenseValue}");
            }
        }

        private void EquipItemMenu(IEnumerable<InventoryItem> inventory)
        {
            Console.Write("Enter item name to equip: ");
            var equipName = Console.ReadLine();
            var equipItem = _inventoryService.SearchByName(inventory, equipName).FirstOrDefault();
            if (equipItem != null)
            {
                EquipItem(equipItem);
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void UseItemMenu(IEnumerable<InventoryItem> inventory)
        {
            Console.Write("Enter item name to use: ");
            var useName = Console.ReadLine();
            var useItem = _inventoryService.SearchByName(inventory, useName).FirstOrDefault();
            if (useItem != null)
            {
                UseItem(useItem);
            }
            else
            {
                Console.WriteLine("Item not found.");
            }
        }

        private void EquipItem(InventoryItem item)
        {
            if (item.AttackValue > 0 || item.DefenseValue > 0)
            {
                Console.WriteLine($"Equipped {item.Name}.");
            }
            else
            {
                Console.WriteLine($"{item.Name} cannot be equipped.");
            }
        }

        private void UseItem(InventoryItem item)
        {
            Console.WriteLine($"Used {item.Name}.");
        }
    }
}

