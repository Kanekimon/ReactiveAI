using System.Collections.Generic;


public class StoreItemsGoals : BaseGoal
{
    public Storage Storage;

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("inventoryFull", false));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        WorldState.AddWorldState("target", Storage.gameObject);
    }


    public override void OnTickGoal()
    {
        if (WorldState.GetValue<bool>("inventoryFull"))
            Prio = MaxPrio;
        else
            Prio = MinPrio;

        base.OnTickGoal();
    }
}

