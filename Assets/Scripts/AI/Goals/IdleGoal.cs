using UnityEngine;


public class IdleGoal : BaseGoal
{

    [SerializeField] int Priority = 10;

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override bool CanRun()
    {
        return true;
    }

}
