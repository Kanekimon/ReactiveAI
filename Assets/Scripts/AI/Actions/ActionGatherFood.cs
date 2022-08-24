using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class ActionGatherFood : ActionBase
{

    private void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasFood", true));
    }

    public override float Cost()
    {
        return _cost;
    }

}

