using System.Collections.Generic;
using UnityEngine;

public class CollectJobResourceGoal : BaseGoal
{
    public Item Gather;
    public GameObject WorkPlace;
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("gatherResource", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        //WorldState.AddWorldState("requestedResource", Gather);
        WorldState.AddWorldState("possibleResources", Agent.Job.GatherThese);
        WorldState.AddWorldState("gatherAmount", Agent.InventorySystem.GetNumberOfFreeSlots() > 0 ? 64 : 0);
        WorkPlace = Agent.Job.Workplace;

    }

    public override void OnGoalDeactivated()
    {
        WorldState.AddWorldState("target", null);
        WorldState.AddWorldState("isAtTarget", false);
        WorldState.AddWorldState("possibleResources", null);
        base.OnGoalDeactivated();
    }


    public override void OnTickGoal()
    {
        if (WorldState.GetValue<bool>("isWorking") && !WorldState.GetValue<bool>("requestResource") && WorldState.GetValue<bool>("isDayTime") && !WorldState.GetValue<bool>("inventoryFull") && WorldState.GetValue<bool>("hasTarget"))
            Prio = MaxPrio;
        else
            Prio = MinPrio;
        base.OnTickGoal();
    }

}
