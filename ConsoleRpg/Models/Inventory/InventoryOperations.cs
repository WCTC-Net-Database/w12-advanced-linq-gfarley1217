using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models.Inventory
{
    public static class InventoryOperations
    {
        public static IEnumerable<InventoryItem> SearchByName(IEnumerable<InventoryItem> inventory, string name)
        {
            return inventory.Where(item => item.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
        }

        public static IEnumerable<IGrouping<string, InventoryItem>> GroupByType(IEnumerable<InventoryItem> inventory)
        {
            return inventory.GroupBy(item => item.Type);
        }

        public static IEnumerable<InventoryItem> SortByName(IEnumerable<InventoryItem> inventory)
        {
            return inventory.OrderBy(item => item.Name);
        }

        public static IEnumerable<InventoryItem> SortByAttackValue(IEnumerable<InventoryItem> inventory)
        {
            return inventory.OrderByDescending(item => item.AttackValue);
        }

        public static IEnumerable<InventoryItem> SortByDefenseValue(IEnumerable<InventoryItem> inventory)
        {
            return inventory.OrderByDescending(item => item.DefenseValue);
        }
    }

}
