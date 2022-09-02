using System.Collections.Generic;
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
        WorldState.AddWorldState("requestedItem", resourceToGather);
        WorldState.AddWorldState("gatherAmount", GatherAmount);
    }

    public override int CalculatePriority()
    {
        return Prio;
    }

    public override void OnGoalDeactivated()
    {
        GatherAmount = 0;
        WorldState.AddWorldState("deliveredResource", false);
        WorldState.AddWorldState("requestedItem", null);
        WorldState.AddWorldState("gatherAmount", 0);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (GatherAmount == 0 || WorldState.GetValue<bool>("deliveredResource"))
            Prio = MinPrio;
        else
            Prio = MaxPrio;

    }
}

