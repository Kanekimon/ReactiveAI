using System.Collections.Generic;

public class GoToWorkAction : ActionBase
{

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isAtWork", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(Agent.Job.Workplace.gameObject, Agent.InteractionRange));
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

