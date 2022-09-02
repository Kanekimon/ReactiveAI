using System.Collections.Generic;


public class ActionGatherFood : ActionBase
{

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isIdle", false));
        effects.Add(new KeyValuePair<string, object>("hasFood", true));
        preconditions.Add(new KeyValuePair<string, object>("interactWithResource", true));
        base.Start();
    }

    public override float Cost()
    {
        return _cost;
    }

}

