using System.Collections.Generic;
using UnityEngine;



public class WanderGoal : BaseGoal
{

    [SerializeField] float PriorityBuildRate = 1;
    [SerializeField] float PriorityDecayRate = 0.1f;
    float CurrentPriority = 0f;

    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isWandering", true));
    }

    public override int CalculatePriority()
    {
        return Mathf.FloorToInt(CurrentPriority);
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {
        if (NavAgent.IsMoving)
            CurrentPriority -= PriorityDecayRate * Time.deltaTime;
        else
            CurrentPriority += PriorityBuildRate * Time.deltaTime;
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        CurrentPriority = MaxPrio;
    }

    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        CurrentPriority = MinPrio;
    }
}

