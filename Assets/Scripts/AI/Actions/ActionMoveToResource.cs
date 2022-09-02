using System.Collections.Generic;
using UnityEngine;

public class ActionMoveToResource : ActionBase
{
    GameObject _target;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasWantedResourceTarget", true));
        effects.Add(new KeyValuePair<string, object>("isAtResource", true));
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


