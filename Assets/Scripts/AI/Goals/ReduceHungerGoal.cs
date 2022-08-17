using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReduceHungerGoal : BaseGoal
{

    [SerializeField] int MinPriority = 0;
    [SerializeField] int MaxPriority = 50;

    Agent Agent;

    private void Awake()
    {
        Agent = GetComponent<Agent>();
    }

    public override int CalculatePriority()
    {
        if ((bool)Agent.GetValueFromMemory("isHungry"))
            return MaxPriority;
        else
            return MinPriority;
    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {
 
    }
}
