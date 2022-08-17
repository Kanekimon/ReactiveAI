﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;



public class WanderGoal : BaseGoal
{
    [SerializeField] int MinPriority = 0;
    [SerializeField] int MaxPriority = 30;

    [SerializeField] float PriorityBuildRate = 1;
    [SerializeField] float PriorityDecayRate = 0.1f;
    float CurrentPriority = 0f;

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

    public override void OnGoalActivated(ActionBase linked)
    {
        base.OnGoalActivated(linked);
        CurrentPriority = MaxPriority;
    }

    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        CurrentPriority = MinPriority;
    }
}

