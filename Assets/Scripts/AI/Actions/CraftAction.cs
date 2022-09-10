using System.Collections.Generic;


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
        return WorldState.GetValue<Request>("activeRequest")?.RequestedItem?.HasRecipe ?? false;
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        Requested = WorldState.GetValue<Request>("activeRequest").RequestedItem;
    }

    public override void OnTick()
    {
        cSystem.CraftItem(Requested);
        OnDeactived();
    }
}

