using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        request = Agent.ReadyForPickUp.Dequeue();
        Agent.WorldState.AddWorldState("interactWithStorage", false);
        Agent.WorldState.AddWorldState("request", request);
        Agent.WorldState.AddWorldState("hasTarget", true);
        Agent.WorldState.AddWorldState("target", request.Storage.gameObject);
        base.OnGoalActivated();
    }

    public override void OnGoalDeactivated()
    {
        Agent.WorldState.RemoveWorldState("interactWithStorage");
        request = null;
        base.OnGoalDeactivated();
    }

}

