using System.Collections.Generic;
using UnityEngine;


public class IdleGoal : BaseGoal
{

    [SerializeField] int Priority = 10;

    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isIdle", true));
    }

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override bool CanRun()
    {
        return true;
    }

}
