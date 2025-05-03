using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models.Inventory
{
    public class InventoryItem
    {
        public string? Name { get; set; }
        public string? Type { get; set; } // e.g., "Weapon", "Armor"
        public int AttackValue { get; set; }
        public int DefenseValue { get; set; }
    }

}
