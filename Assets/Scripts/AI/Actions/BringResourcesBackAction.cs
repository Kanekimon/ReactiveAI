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
        Item item = WorldState.GetValue<Item>("requestedResource");
        InventoryItem gathered = Agent.InventorySystem.GetInventoryItem(item);
        WorldState.GetValue<GameObject>("target").GetComponent<Storage>().PutIntoStorage(gathered.Item, gathered.Amount);
        Agent.InventorySystem.RemoveItem(item, gathered.Amount);
    }

}

