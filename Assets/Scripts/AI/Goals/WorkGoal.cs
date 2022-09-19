using System.Collections.Generic;

public class WorkGoal : BaseGoal
{

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("hasTool", true));
        //Preconditions.Add(new KeyValuePair<string, object>("gatherResource", true));
        base.Start();
    }

    public override void OnGoalDeactivated()
    {
        WorldState.AddWorldState("isWorking", true);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (Agent.Job.Tools.Count == 0)
            WorldState.AddWorldState("isWorking", true);

        if (Agent.Job != null && Agent.Job.Tools.Count > 0 && !WorldState.GetValue<bool>("isWorking") && !WorldState.GetValue<bool>("hasTool") && WorldState.GetValue<DayTime>("dayTime") == DayTime.Morning)
            Prio = MaxPrio;
        else
            Prio = MinPrio;
    }

}

