using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionSearchResource : ActionBase
{
    ResourceType _resourceToSearch;

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasWantedResourceTarget", true));        base.Start();
    }

    protected override void Awake()
    {
        base.Awake();
    }

    public override void OnTick()
    {
        if (NavAgent.AtDestination)
            OnActivated(LinkedGoal);

        if (Agent.AwarenessSystem.KnowsResourceOfType(_resourceToSearch))
        {
            OnDeactived();
        }

        //if (Agent.AwarenessSystem.ResourceNodesInRange.Any(a => ((ResourceTarget)a.Detectable).ResourceType == ResourceType.Food))
        //{
        //    float closestDistance = float.MaxValue;
        //    TrackedTarget r = null;

        //    foreach (var resource in Agent.AwarenessSystem.ResourceNodesInRange.Where(a => ((ResourceTarget)a.Detectable).ResourceType == ResourceType.Food))
        //    {
        //        var distance = Vector3.Distance(resource.RawPosition, Agent.transform.position);
        //        if (distance < closestDistance)
        //        {
        //            closestDistance = distance;
        //            r = resource;
        //        }
        //    }

        //    if (r != null)
        //    {
        //        Agent.SaveValueInMemory("targetLocation", r.RawPosition);
        //    }
        //}
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);

        _resourceToSearch = (ResourceType)Enum.Parse(typeof(ResourceType), Agent.WorldState.GetValue("resourceToGather").ToString());

        Vector3 location = NavAgent.PickLocationInRange(10f);
        NavAgent.MoveTo(location);
    }

    public override void OnDeactived()
    {
        NavAgent.StopMoving();
        _hasFinished = true;
    }

}

