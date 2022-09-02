using System.Collections.Generic;


public class ChopTreeGoal : BaseGoal
{
    public Item Gather;
    public Storage Storage;
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("gatherResource", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        WorldState.AddWorldState("requestedResource", Gather);
        WorldState.AddWorldState("gatherAmount", Agent.InventorySystem.GetFreeSpaceSize());


    }

    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
    }


    public override void OnTickGoal()
    {
        if (WorldState.GetValue<bool>("isDayTime") && !WorldState.GetValue<bool>("inventoryFull"))
            Prio = MaxPrio;
        else
            Prio = MinPrio;
        base.OnTickGoal();
    }

}
