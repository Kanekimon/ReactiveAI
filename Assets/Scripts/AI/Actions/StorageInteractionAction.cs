using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

internal class StorageInteractionAction : ActionBase
{
    Request request;
    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("isAtPosition", true));
        effects.Add(new KeyValuePair<string, object>("interactWithStorage", true));

        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        request = Agent.WorldState.GetValue("request") as Request;
    }

    public override void OnTick()
    {
        base.OnTick();
        request.Storage.RetrieveFromStorage(Agent.InventorySystem, request.RequestedItem, request.RequestedAmount);
        OnDeactived();
    }

}

