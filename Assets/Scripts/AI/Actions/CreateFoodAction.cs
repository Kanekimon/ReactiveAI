using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


internal class CreateFoodAction : ActionBase
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