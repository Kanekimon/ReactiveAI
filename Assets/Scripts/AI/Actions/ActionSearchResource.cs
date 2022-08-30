using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionSearchResource : ActionBase
{
    Item _item;

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasWantedResourceTarget", true));        
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
    }



    public override void OnTick()
    {
        if (NavAgent.AtDestination)
            OnActivated(LinkedGoal);

        if (Agent.AwarenessSystem.KnowsResourceOfType(_item))
        {
            OnDeactived();
        }
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);

        _item = Agent.WorldState.GetValue("resourceToGather") as Item;

        Vector3 location = NavAgent.PickLocationInRange(10f);
        NavAgent.MoveTo(location);
    }

    public override void OnDeactived()
    {
        NavAgent.StopMoving();
        _hasFinished = true;
    }

}

