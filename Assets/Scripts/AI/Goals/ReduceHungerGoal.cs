using System.Collections.Generic;
using UnityEngine;

public class ReduceHungerGoal : BaseGoal
{

    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isHungry", false));
    }

    public override int CalculatePriority()
    {
        if ((bool)Agent.WorldState.GetValue("isHungry"))
            return MaxPrio;
        else
            return MinPrio;
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();

        Agent.WorldState.AddWorldState("requestedItem", ItemManager.Instance.GetItemByName("food"));

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {

    }

}
