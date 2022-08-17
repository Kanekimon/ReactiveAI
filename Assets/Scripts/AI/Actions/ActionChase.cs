using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionChase : ActionBase
{
    [SerializeField] float SearchRange = 10f;
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(ChaseGoal) };

    ChaseGoal chaseGoal;

    public override List<System.Type> GetSupportedGoals()
    {
        return SupportedGoals;
    }

    public override float Cost()
    {
        return 0f;
    }

    public override void OnTick()
    {
        NavAgent.MoveTo(chaseGoal.MoveLocation);
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        chaseGoal = (ChaseGoal) LinkedGoal;
        NavAgent.MoveTo(chaseGoal.MoveLocation);
    }

    public override void OnDeactived()
    {
        chaseGoal = null;
    }
}
