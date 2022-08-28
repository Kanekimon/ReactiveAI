using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActionDeliverToStorage : ActionBase
{
    TownSystem town;
    GameObject targetStorage;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasResource", true));
        effects.Add(new KeyValuePair<string, object>("deliveredResource", true));

        town = Agent.HomeTown;
        targetStorage = town.GetStorage(ResourceType.Wood);

        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(targetStorage, 1f));
    }

    public override void OnTick()
    {
        if (targetStorage != null)
        {
            if (NavAgent.AtDestination)
            {
                Item toDeliver = ItemManager.Instance.GetItemByName(Agent.WorldState.GetValue("resourceToGather").ToString().ToLower());
                int amount = int.Parse(Agent.WorldState.GetValue("gatherAmount").ToString());
                Agent.InventorySystem.TransferToOther(targetStorage.GetComponent<InventorySystem>(), toDeliver, amount);
                Agent.WorldState.AddWorldState("deliveredResource", true);
                OnDeactived();
            }
            else
                NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(targetStorage, 1f));
        }
        else
        {
            OnDeactived();
        }
    }

}

