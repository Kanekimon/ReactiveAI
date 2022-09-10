using System.Collections.Generic;

public class CraftGoal : BaseGoal
{
    public Recipe RequestedRecipe;

    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("deliveredItem", true));
        base.Start();
    }

    public override void OnGoalActivated()
    {

    }


    public override int CalculatePriority()
    {
        return Prio;
    }


    public override void OnTickGoal()
    {
        if (RequestedRecipe != null)
            Prio = MaxPrio;
        else
            Prio = MinPrio;
    }
}

