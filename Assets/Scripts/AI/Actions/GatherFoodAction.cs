using System.Collections.Generic;


public class GatherFoodAction : ActionBase
{

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isIdle", false));
        effects.Add(new KeyValuePair<string, object>("hasFood", true));
        preconditions.Add(new KeyValuePair<string, object>("interactWithResource", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);

    }

    public override void OnTick()
    {
        base.OnTick();
        OnDeactived();
    }

    public override float Cost()
    {
        return _cost;
    }

}

