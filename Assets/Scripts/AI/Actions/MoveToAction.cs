using System.Collections.Generic;
using UnityEngine;

internal class MoveToAction : ActionBase
{
    GameObject _target;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasTarget", true));
        effects.Add(new KeyValuePair<string, object>("isAtPosition", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        _target = WorldState.GetValue<GameObject>("target");
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(_target, 5f));
    }

    public override void OnTick()
    {
        base.OnTick();
        if (NavAgent.AtDestination)
        {
            OnDeactived();
        }
    }

}

