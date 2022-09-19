using System.Collections.Generic;
using UnityEngine;

public class FindResourceGoal : BaseGoal
{
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("hasTarget", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        WorldState.AddWorldState("possibleResources", Agent.Job.GatherThese);
        base.OnGoalActivated();
    }


    public override void OnTickGoal()
    {
        if (!WorldState.GetValue<bool>("hasTarget") && WorldState.GetValue<bool>("isWorking"))
        {
            Prio = MaxPrio;
        }
        else if (WorldState.GetValue<bool>("hasTarget") && WorldState.GetValue<bool>("isWorking"))
        {
            GameObject target = WorldState.GetValue<GameObject>("target");

            if (target == null || target.GetComponent<ResourceTarget>() == null || !Agent.Job.GatherThese.Contains(target.GetComponent<ResourceTarget>().ResourceType))
                Prio = MaxPrio;
            else
                Prio = MinPrio;

        }
        else
            Prio = MinPrio;


    }

}

