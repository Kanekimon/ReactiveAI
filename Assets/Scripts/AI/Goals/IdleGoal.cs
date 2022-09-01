using System.Collections.Generic;
using UnityEngine;


public class IdleGoal : BaseGoal
{
    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isIdle", true));
    }

    public override int CalculatePriority()
    {
        return Prio;
    }

    public override bool CanRun()
    {
        return true;
    }

}
