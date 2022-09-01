﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class DeliverItemGoal : BaseGoal
{
    [SerializeField] public int GatherAmount;
    [SerializeField] Item resourceToGather;

    public Item ItemToGather => resourceToGather;


    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("deliveredItem", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        Agent.WorldState.AddWorldState("requestedItem", resourceToGather);
        Agent.WorldState.AddWorldState("gatherAmount", GatherAmount);
    }

    public override int CalculatePriority()
    {
        return Prio;
    }

    public override void OnGoalDeactivated()
    {
        GatherAmount = 0;
        Agent.WorldState.AddWorldState("deliveredResource", false);
        Agent.WorldState.AddWorldState("requestedItem", null);
        Agent.WorldState.AddWorldState("gatherAmount", 0);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (GatherAmount == 0 || (bool)Agent.WorldState.GetValue("deliveredResource"))
            Prio = MinPrio;
        else
            Prio = MaxPrio;

    }
}

