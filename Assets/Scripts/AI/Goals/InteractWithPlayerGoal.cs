using System.Collections.Generic;


public class InteractWithPlayerGoal : BaseGoal
{
    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isIdle", true));
        base.Start();
    }


    public override void OnGoalActivated()
    {
        base.OnGoalActivated();
    }

    public override void OnTickGoal()
    {
        if (WorldState.GetValue<bool>("playerWantsInteraction"))
        {
            Prio = MaxPrio;
        }
        else
            Prio = MinPrio;
    }

}

