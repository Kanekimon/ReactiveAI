using System.Collections.Generic;


public class RetrieveRequestedItemGoal : BaseGoal
{
    Request request;
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("interactWithStorage", true));
        base.Start();
    }

    public override void OnTickGoal()
    {
        base.OnTickGoal();

        if (Agent.ReadyForPickUp.Count > 0)
            Prio = MaxPrio;
        else
            Prio = MinPrio;

        if (request != null)
        {
            if (request.Status == RequestStatus.Finished)
                Prio = MinPrio;
        }

    }

    public override int CalculatePriority()
    {
        return Prio;
    }

    public override void OnGoalActivated()
    {
        request = Agent.ReadyForPickUp.Peek();
        WorldState.AddWorldState("interactWithStorage", false);
        WorldState.AddWorldState("request", request);
        WorldState.AddWorldState("hasTarget", true);
        WorldState.AddWorldState("target", Agent.HomeTown.RequestBoard.gameObject);
        base.OnGoalActivated();
    }

    public override void OnGoalDeactivated()
    {
        WorldState.RemoveWorldState("interactWithStorage");
        request = null;
        Agent.WorldState.AddWorldState("requestResource", false);
        base.OnGoalDeactivated();
    }

}

