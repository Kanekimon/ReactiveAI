using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class RetrieveRequestedItemGoal : BaseGoal
{
    Request request;
    public int Priority;
    public int MinPrio;
    public int MaxPrio;

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("interactWithStorage", true));
        base.Start();
    }

    public override void OnTickGoal()
    {
        base.OnTickGoal();

        if (request != null || Agent.FinishedRequests.Count > 0)
            Priority = MaxPrio;
        else
            Priority = MinPrio;
    }

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override void OnGoalActivated()
    {

        request = Agent.FinishedRequests.Dequeue();
        Agent.WorldState.AddWorldState("interactWithStorage", false);
        Agent.WorldState.AddWorldState("request", request);
        Agent.WorldState.AddWorldState("hasTarget", true);
        Agent.WorldState.AddWorldState("target", request.Storage.gameObject);
        //Agent.WorldState.AddWorldState("target", );

        base.OnGoalActivated();
    }

    public override void OnGoalDeactivated()
    {
        Agent.WorldState.RemoveWorldState("interactWithStorage");
        request = null;
        base.OnGoalDeactivated();
    }

}

