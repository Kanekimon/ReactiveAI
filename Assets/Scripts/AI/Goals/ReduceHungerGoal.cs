using System.Collections.Generic;

public class ReduceHungerGoal : BaseGoal
{

    private void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("isHungry", false));
    }

    public override int CalculatePriority()
    {
        if (WorldState.GetValue<bool>("isHungry"))
            return MaxPrio;
        else
            return MinPrio;
    }

    public override void OnGoalActivated()
    {
        base.OnGoalActivated();

        WorldState.AddWorldState("requestedItem", ItemManager.Instance.GetItemByName("food"));

    }

    public override bool CanRun()
    {
        return true;
    }

    public override void OnTickGoal()
    {

    }

}
