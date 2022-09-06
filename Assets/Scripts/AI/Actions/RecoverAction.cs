using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RecoverAction : ActionBase
{
    ConditionSystem CSystem;
    float timer = 0;
    float delay = 2f;

    protected override void Start()
    {
        CSystem = Agent.ConditionSystem;
        effects.Add(new KeyValuePair<string, object>("isExhausted", false));
        //preconditions.Add(new KeyValuePair<string, object>("isHome", true));
        base.Start();
    }


    public override void OnTick()
    {
        base.OnTick();
        if(timer > delay)
        {
            timer = 0;
            CSystem.ChangeValue("Exhausted", 1f);
        }
        timer += Time.deltaTime;
        if (CSystem.GetActiveCondition() == null)
        {
            OnDeactived();
        }

    }

}

