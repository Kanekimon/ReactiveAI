using System.Collections.Generic;
using UnityEngine;



public class WanderGoal : BaseGoal
{

    [SerializeField] float PriorityBuildRate = 1f;
    [SerializeField] float PriorityDecayRate = 0.1f;
    [SerializeField] float CurrentPrio;

    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isWandering", true));
    }

    public override int CalculatePriority()
    {
        Prio = Mathf.FloorToInt(CurrentPrio);
        return Prio;
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {
        if (NavAgent.IsMoving)
            CurrentPrio = Mathf.Clamp(CurrentPrio - (PriorityDecayRate * Time.deltaTime), MinPrio, MaxPrio);
        else
            CurrentPrio = Mathf.Clamp(CurrentPrio + (PriorityBuildRate * Time.deltaTime), MinPrio, MaxPrio);
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
        Prio = MaxPrio;
        CurrentPrio = MaxPrio;
    }

    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        Prio = MinPrio;
        CurrentPrio = MinPrio;
    }
}

