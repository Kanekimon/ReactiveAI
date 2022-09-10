using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class SleepAction : ActionBase
{
    ConditionSystem CSystem;
    float timer = 0;
    float delay = 2f;

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("isSleeping", true));
        preconditions.Add(new KeyValuePair<string, object>("isHome", true));
        base.Start();
    }


    public override void OnTick()
    {
        base.OnTick();
        if(WorldState.GetValue<DayTime>("DayTime") == DayTime.Morning)
        {
            OnDeactived();
        }
    }

}
