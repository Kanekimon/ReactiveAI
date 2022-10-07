using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetAction : ActionBase
{
    GameObject _target;
    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isAtTarget", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        _target = WorldState.GetValue<GameObject>("target");
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(_target, Agent.InteractionRange));
    }

    public override void OnTick()
    {
        base.OnTick();
        if (NavAgent.AtDestination)
        {
            NavAgent.StopMoving();
            OnDeactived();
        }
    }


}

