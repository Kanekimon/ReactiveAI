using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class CreateFoodAction : ActionBase
{

    
    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasFood", true));
        base.Start();
    }

    public override float Cost()
    {
        return _cost;
    }


}