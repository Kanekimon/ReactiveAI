using System.Collections.Generic;
using UnityEngine;

public class ActionWander : ActionBase
{
    [SerializeField] float SearchRange = 10f;


    protected override void Awake()
    {
        base.Awake();
    }
    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isWandering", true));

        base.Start();
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
        _hasFinished = false;
        NavAgent.StopMoving();
    }
}
