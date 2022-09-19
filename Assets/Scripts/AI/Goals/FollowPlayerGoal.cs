using System.Collections.Generic;


public class FollowPlayerGoal : BaseGoal
{
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isAtPosition", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {
        WorldState.AddWorldState("target", GameManager.Instance.Player.gameObject);
        WorldState.AddWorldState("hasTarget", true);
        base.OnGoalActivated();
    }

    public override void OnTickGoal()
    {
        base.OnTickGoal();
        if (WorldState.GetValue<bool>("followPlayer"))
            Prio = MaxPrio;
        else
            Prio = MinPrio;

    }

}
