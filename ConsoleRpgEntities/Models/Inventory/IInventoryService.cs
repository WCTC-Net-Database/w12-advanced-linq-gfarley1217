using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleRpgEntities.Models.Inventory
{
    public interface IInventoryService
    {
        IEnumerable<InventoryItem> SearchByName(IEnumerable<InventoryItem> inventory, string name);
        IEnumerable<IGrouping<string, InventoryItem>> GroupByType(IEnumerable<InventoryItem> inventory);
        IEnumerable<InventoryItem> SortByName(IEnumerable<InventoryItem> inventory);
        IEnumerable<InventoryItem> SortByAttackValue(IEnumerable<InventoryItem> inventory);
        IEnumerable<InventoryItem> SortByDefenseValue(IEnumerable<InventoryItem> inventory);
    }

}
