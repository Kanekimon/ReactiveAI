using System.Collections.Generic;
using UnityEngine;

public class DeliverToRequestBoardAction : ActionBase
{
    TownSystem town;
    GameObject target;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasItem", true));
        effects.Add(new KeyValuePair<string, object>("deliveredItem", true));

        town = Agent.HomeTown;
        target = town.RequestBoard.gameObject;

        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(target, Agent.InteractionRange));
    }

    public override void OnTick()
    {
        if (target != null)
        {
            if (NavAgent.AtDestination)
            {
                Request r = WorldState.GetValue<Request>("activeRequest");
                Item toDeliver = r.RequestedItem;
                int amount = r.RequestedAmount;
                if (Agent.InventorySystem.TransferItemToOtherInventory(target.GetComponent<InventorySystem>(), toDeliver, amount))
                {
                    town.FinishedRequest(Agent, toDeliver, target);
                    WorldState.AddWorldState("deliveredResource", true);
                    Agent.WorkingOn = null;
                    WorldState.AddWorldState("activeRequest", Agent.WorkingOn);
                }
                OnDeactived();
            }
        }
        else
        {
            OnDeactived();
        }
    }

}

