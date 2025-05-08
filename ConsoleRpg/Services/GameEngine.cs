using ConsoleRpg.Helpers;
using ConsoleRpgEntities.Data;
using ConsoleRpgEntities.Models.Attributes;
using ConsoleRpgEntities.Models.Characters;
using ConsoleRpgEntities.Models.Characters.Monsters;
using ConsoleRpgEntities.Models.Inventory;
using ConsoleRpgEntities.Models.Abilities.PlayerAbilities; // Add this line
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;

namespace ConsoleRpg.Services
{
    public class GameEngine
    {
        private readonly GameContext _context;
        private readonly ILogger<GameEngine> _logger;

        public GameEngine(GameContext context, ILogger<GameEngine> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Run()
        {
            Console.WriteLine("GameEngine is running...");

            while (true)
            {
                Console.WriteLine("Main Menu:");
                Console.WriteLine("1. Add a new character");
                Console.WriteLine("2. Edit an existing character");
                Console.WriteLine("3. Execute an ability during an attack");
                Console.WriteLine("4. Exit");
                Console.Write("Select an option: ");
                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddCharacter();
                        break;

                    case "2":
                        EditCharacter();
                        break;

                    case "3":
                        ExecuteAbilityDuringAttack();
                        break;

                    case "4":
                        return;

                    default:
                        Console.WriteLine("Invalid option. Try again.");
                        break;
                }
            }
        }

        private void AddCharacter()
        {
            Console.WriteLine("Enter details for the new character:");

            Console.Write("Name: ");
            var name = Console.ReadLine();

            Console.Write("Health: ");
            if (!int.TryParse(Console.ReadLine(), out var health))
            {
                Console.WriteLine("Invalid input for Health. Operation canceled.");
                return;
            }

            Console.Write("Attack: ");
            if (!int.TryParse(Console.ReadLine(), out var attack))
            {
                Console.WriteLine("Invalid input for Attack. Operation canceled.");
                return;
            }

            Console.Write("Defense: ");
            if (!int.TryParse(Console.ReadLine(), out var defense))
            {
                Console.WriteLine("Invalid input for Defense. Operation canceled.");
                return;
            }

            var newCharacter = new Player
            {
                Name = name,
                Health = health,
                Experience = 0,
                EquipmentId = null, // No equipment by default
                Inventory = new Inventory(),
                Abilities = new List<Ability>()
            };

            _context.Players.Add(newCharacter);
            _context.SaveChanges();

            _logger.LogInformation($"Character '{name}' added successfully.");
            Console.WriteLine($"Character '{name}' added successfully!");
        }

        private void EditCharacter()
        {
            Console.WriteLine("Enter the ID of the character to edit:");
            if (!int.TryParse(Console.ReadLine(), out var id))
            {
                Console.WriteLine("Invalid input for ID. Operation canceled.");
                return;
            }

            var character = _context.Players.FirstOrDefault(p => p.Id == id);
            if (character == null)
            {
                Console.WriteLine("Character not found.");
                return;
            }

            Console.WriteLine($"Editing character: {character.Name}");

            Console.Write("New Health (leave blank to keep current): ");
            var healthInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(healthInput) && int.TryParse(healthInput, out var newHealth))
            {
                character.Health = newHealth;
            }

            Console.Write("New Attack (leave blank to keep current): ");
            var attackInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(attackInput) && int.TryParse(attackInput, out var newAttack))
            {
                character.Equipment.Weapon.Attack = newAttack;
            }

            Console.Write("New Defense (leave blank to keep current): ");
            var defenseInput = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(defenseInput) && int.TryParse(defenseInput, out var newDefense))
            {
                character.Equipment.Armor.Defense = newDefense;
            }

            _context.SaveChanges();

            _logger.LogInformation($"Character '{character.Name}' updated successfully.");
            Console.WriteLine($"Character '{character.Name}' updated successfully!");
        }

        private void ExecuteAbilityDuringAttack()
        {
            Console.WriteLine("Enter the ID of the attacking character:");
            if (!int.TryParse(Console.ReadLine(), out var attackerId))
            {
                Console.WriteLine("Invalid input for ID. Operation canceled.");
                return;
            }

            var attacker = _context.Players.Include(p => p.Abilities).FirstOrDefault(p => p.Id == attackerId);
            if (attacker == null)
            {
                Console.WriteLine("Attacking character not found.");
                return;
            }

            Console.WriteLine("Enter the ID of the target character:");
            if (!int.TryParse(Console.ReadLine(), out var targetId))
            {
                Console.WriteLine("Invalid input for ID. Operation canceled.");
                return;
            }

            var target = _context.Players.FirstOrDefault(p => p.Id == targetId);
            if (target == null)
            {
                Console.WriteLine("Target character not found.");
                return;
            }

            Console.WriteLine($"Attacker: {attacker.Name}, Target: {target.Name}");
            Console.WriteLine("Available Abilities:");
            for (int i = 0; i < attacker.Abilities.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {attacker.Abilities.ElementAt(i).Name}");
            }

            Console.Write("Select an ability to use: ");
            if (!int.TryParse(Console.ReadLine(), out var abilityIndex) || abilityIndex < 1 || abilityIndex > attacker.Abilities.Count)
            {
                Console.WriteLine("Invalid ability selection. Operation canceled.");
                return;
            }

            var selectedAbility = attacker.Abilities.ElementAt(abilityIndex - 1);
            selectedAbility.Activate(attacker, target);

            Console.WriteLine($"Ability '{selectedAbility.Name}' executed by {attacker.Name} on {target.Name}.");
            _logger.LogInformation($"Ability '{selectedAbility.Name}' executed by {attacker.Name} on {target.Name}.");
        }
    }
}
