using System.Collections.Generic;


public class EndDayGoal : BaseGoal
{
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("hasTool", false));
        Preconditions.Add(new KeyValuePair<string, object>("isSleeping", true));
        base.Start();
    }

    public override void OnGoalDeactivated()
    {
        WorldState.AddWorldState("startedWork", false);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (WorldState.GetValue<bool>("isDayTime"))
            Prio = MinPrio;
        else
            Prio = MaxPrio;
    }


}

