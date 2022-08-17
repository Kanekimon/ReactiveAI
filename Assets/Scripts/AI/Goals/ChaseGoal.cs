using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ChaseGoal : BaseGoal
{
    [SerializeField] int ChasePriority = 60;
    [SerializeField] float MinAwarenessToChase = 1.5f;
    [SerializeField] float AwarenessToStopChase = 1f;

    DetectableTarget CurrentTarget;

    int CurrenPriority;

    public Vector3 MoveLocation => CurrentTarget != null ? CurrentTarget.transform.position : transform.position;


    public override bool CanRun()
    {
        if(AwarenessSystem.ActiveTargets == null || AwarenessSystem.ActiveTargets.Count == 0)
            return false;


        //If aware of something
        foreach(var target in AwarenessSystem.ActiveTargets)
        {
            if(target.Awareness > MinAwarenessToChase)
                return true;
        }

        return true;
    }

    public override void OnTickGoal()
    {
        CurrenPriority = 0;

        if (AwarenessSystem.ActiveTargets == null || AwarenessSystem.ActiveTargets.Count == 0)
            return;

        if (CurrentTarget != null)
        {
            foreach (var target in AwarenessSystem.ActiveTargets)
            {
                if (target.Detectable == CurrentTarget)
                {
                    CurrenPriority = target.Awareness < AwarenessToStopChase ? 0 : ChasePriority;
                    return;
                }
            }

            //Clear if target is not relevant anymore
            CurrentTarget = null;
        }

        //require new target if possible
        foreach (var target in AwarenessSystem.ActiveTargets)
        {
            if (target.Awareness >= MinAwarenessToChase)
            {
                CurrentTarget = target.Detectable;
                CurrenPriority = ChasePriority;
                return;
            }
        }
    }

    public override int CalculatePriority()
    {
        return CurrenPriority;
    }

    public override void OnGoalDeactivated()
    {
        base.OnGoalDeactivated();
        CurrentTarget = null;
    }
}

