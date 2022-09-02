using System.Collections.Generic;


public class ActionDummy : ActionBase
{

    protected override void Start()
    {

        preconditions.Add(new KeyValuePair<string, object>("hasFood", true));
        preconditions.Add(new KeyValuePair<string, object>("isIdle", true));
        effects.Add(new KeyValuePair<string, object>("isHungry", false));
        base.Start();
    }

    public override float Cost()
    {
        return _cost;
    }


}

