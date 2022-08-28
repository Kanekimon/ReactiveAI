using System.Collections.Generic;
using UnityEngine;

public class ReduceHungerGoal : BaseGoal
{

    [SerializeField] int MinPriority = 0;
    [SerializeField] int MaxPriority = 50;
    

    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isHungry", false));
    }

    public override int CalculatePriority()
    {
        if ((bool)Agent.WorldState.GetValue("isHungry"))
            return MaxPriority;
        else
            return MinPriority;
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();

        Agent.WorldState.AddWorldState("resourceToGather", ResourceType.Food);

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {

    }

}
