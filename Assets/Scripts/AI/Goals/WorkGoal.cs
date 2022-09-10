using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class WorkGoal : BaseGoal
{

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("hasTool",true));
        //Preconditions.Add(new KeyValuePair<string, object>("gatherResource", true));
        base.Start();
    }

    public override void OnGoalDeactivated()
    {
        WorldState.AddWorldState("startedWork", true);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (!WorldState.GetValue<bool>("startedWork") && !WorldState.GetValue<bool>("hasTool") && WorldState.GetValue<DayTime>("dayTime") == DayTime.Morning)
            Prio = MaxPrio;
        else
            Prio = MinPrio;
    }

}

