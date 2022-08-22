using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionWander : ActionBase
{
    [SerializeField] float SearchRange = 10f;
    List<System.Type> SupportedGoals = new List<System.Type>() { typeof(WanderGoal) };

    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        base.Start();
    }

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
        if (NavAgent.AtDestination)
            OnActivated(LinkedGoal);
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        Vector3 location = NavAgent.PickLocationInRange(SearchRange);
        NavAgent.MoveTo(location);
    }

    public override void OnDeactived()
    {
        NavAgent.StopMoving();
    }
}
