using System.Collections.Generic;


public class GoHomeAction : ActionBase
{

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isHome", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(Agent.Home.gameObject, Agent.InteractionRange));
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

