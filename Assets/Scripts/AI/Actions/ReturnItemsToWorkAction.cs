using System.Collections.Generic;
using System.Linq;


public class ReturnItemsToWorkAction : ActionBase
{
    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasTool", false));
        effects.Add(new KeyValuePair<string, object>("inventoryFull", false));
        preconditions.Add(new KeyValuePair<string, object>("isAtWork", true));
        base.Start();
    }

    public override void OnTick()
    {
        base.OnTick();

        List<InventoryItem> allItems = Agent.InventorySystem.GetAllItems();
        if (WorldState.GetValue<DayTime>("dayTime") != DayTime.Evening)
        {
            allItems = allItems.Where(a => !a.Item.ItemTypes.Contains(ItemType.Tool)).ToList();
        }
        foreach (InventoryItem item in allItems)
        {
            Agent.InventorySystem.TransferItemToOtherInventory(Agent.Job.Workplace.GetComponent<InventorySystem>(), item.Item, item.Amount);
        }


        OnDeactived();
    }

}

