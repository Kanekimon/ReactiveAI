using System.Collections.Generic;
using UnityEngine;

public class BringResourcesBackAction : ActionBase
{
    protected override void Start()
    {

        effects.Add(new KeyValuePair<string, object>("inventoryFull", false));
        preconditions.Add(new KeyValuePair<string, object>("isAtTarget", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);

    }

    public override void OnTick()
    {
        base.OnTick();
        foreach (InventoryItem invItem in Agent.InventorySystem.GetAllItems())
        {
            if (invItem == null || (invItem != null && invItem.Item.ItemTypes.Contains(ItemType.Tool)))
                continue;

            Agent.InventorySystem.TransferItemToOtherInventory(WorldState.GetValue<GameObject>("target").GetComponent<InventorySystem>(), invItem.Item, invItem.Amount);
        }
        OnDeactived();

    }

}

