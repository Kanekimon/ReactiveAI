using System.Collections.Generic;
using UnityEngine;

public class ActionDeliverToStorage : ActionBase
{
    TownSystem town;
    GameObject targetStorage;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasItem", true));
        effects.Add(new KeyValuePair<string, object>("deliveredItem", true));

        town = Agent.HomeTown;


        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        targetStorage = town.GetStorage(WorldState.GetValue<Item>("requestedItem"));
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(targetStorage, Agent.InteractionRange));
    }

    public override void OnTick()
    {
        if (targetStorage != null)
        {
            if (NavAgent.AtDestination)
            {
                Item toDeliver = WorldState.GetValue<Item>("requestedItem") as Item;
                int amount = WorldState.GetValue<int>("gatherAmount");
                Agent.InventorySystem.TransferToOther(targetStorage.GetComponent<InventorySystem>(), toDeliver, amount);
                town.FinishedRequest(Agent, toDeliver, targetStorage);
                WorldState.AddWorldState("deliveredResource", true);
                OnDeactived();
            }
        }
        else
        {
            OnDeactived();
        }
    }

}

