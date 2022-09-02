using System.Collections.Generic;
using UnityEngine;

public class ActionChase : ActionBase
{
    [SerializeField] float SearchRange = 10f;
    GameObject target;


    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("hasTargets", true));
        effects.Add(new KeyValuePair<string, object>("isChasing", true));
        base.Start();
    }


    public override float Cost()
    {
        return 0f;
    }

    public override void OnTick()
    {
        if (target != null)
            NavAgent.MoveTo(target.transform.position);
        else
        {
            _hasFinished = true;
            OnDeactived();
        }
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        target = WorldState.GetValue<GameObject>("target");
    }

    public override void OnDeactived()
    {
        base.OnDeactived();
    }
}
