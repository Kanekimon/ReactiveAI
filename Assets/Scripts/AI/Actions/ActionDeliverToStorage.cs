﻿using System;
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


        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        targetStorage = town.GetStorage(Agent.WorldState.GetValue("resourceToGather") as Item);
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(targetStorage, 1f));
    }

    public override void OnTick()
    {
        if (targetStorage != null)
        {
            if (NavAgent.AtDestination)
            {
                Item toDeliver = Agent.WorldState.GetValue("resourceToGather") as Item;
                int amount = int.Parse(Agent.WorldState.GetValue("gatherAmount").ToString());
                Agent.InventorySystem.TransferToOther(targetStorage.GetComponent<InventorySystem>(), toDeliver, amount);
                town.FinishedRequest(Agent, toDeliver, targetStorage);
                Agent.WorldState.AddWorldState("deliveredResource", true);
                OnDeactived();
            }
        }
        else
        {
            OnDeactived();
        }
    }

}
