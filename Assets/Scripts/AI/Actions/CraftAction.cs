using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


public class CraftAction : ActionBase
{
    Item Requested;
    CraftingSystem cSystem;

    protected override void Start()
    {
        cSystem = Agent.CraftingSystem;
        effects.Add(new KeyValuePair<string, object>("hasItem", true));
        preconditions.Add(new KeyValuePair<string, object>("hasMaterials", true));
        base.Start();
    }


    public override bool CanRun()
    {
        return base.CanRun();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        Requested = Agent.WorldState.GetValue("requestedItem") as Item;
    }

    public override void OnTick()
    {
        cSystem.CraftItem(Requested);
    }
}

