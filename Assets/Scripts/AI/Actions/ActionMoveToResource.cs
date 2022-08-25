using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActionMoveToResource : ActionBase
{
    GameObject _target;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasResourceTargets", true));
        effects.Add(new KeyValuePair<string, object>("isAtResource", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        _target = Agent.GetValueFromMemory("resourceTarget") as GameObject;
        NavAgent.MoveTo(NavAgent.PickLocationNearTarget(_target.transform.position,10f));
    }

    public override void OnTick()
    {
        base.OnTick();
        if (NavAgent.AtDestination)
            OnDeactived();
    }


}


