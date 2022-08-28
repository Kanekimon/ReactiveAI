﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class GetResourceGoal : BaseGoal
{

    [SerializeField] int Priority = 10;
    [SerializeField] int MinPrio = 1;
    [SerializeField] int MaxPrio = 100;
    [SerializeField] public int GatherAmount;
    [SerializeField] ResourceType resourceToGather;

    public ResourceType ResourceToGather => resourceToGather;


    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("interactWithResource", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        Agent.WorldState.AddWorldState("resourceToGather", resourceToGather);
        Agent.WorldState.AddWorldState("gatherAmount", GatherAmount);
    }

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override void OnGoalDeactivated()
    {
        GatherAmount = 0;
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {   
        if (GatherAmount == 0 || Agent.InventorySystem.HasEnoughOfResource(resourceToGather, GatherAmount))
            Priority = MinPrio;
        else
            Priority = MaxPrio;

    }
}
