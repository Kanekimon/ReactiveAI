using System.Collections.Generic;
using UnityEngine;

public class ActionSearchResource : ActionBase
{
    Item _item;

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasTarget", true));
        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
    }



    public override void OnTick()
    {
        if (NavAgent.AtDestination)
        {
            OnActivated(LinkedGoal);
        }


        if (Agent.AwarenessSystem.SeesResourceTarget(WorldState.GetValue<List<ResourceType>>("possibleResources")))
        {
            OnDeactived();
        }

        //if (Agent.AwarenessSystem.KnowsResourceOfType(_item))
        //{
        //    OnDeactived();
        //}
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);

        NavAgent.RotateTowards(Agent.transform.position + Vector3.forward);
        NavAgent.RotateTowards(Agent.transform.position + Vector3.left);
        NavAgent.RotateTowards(Agent.transform.position + Vector3.right);

        _item = WorldState.GetValue<Item>("requestedResource");

        Vector3 location = NavAgent.PickLocationInRange(10f);
        NavAgent.MoveTo(location);
    }

    public override void OnDeactived()
    {
        NavAgent.StopMoving();
        _hasFinished = true;
    }

}

