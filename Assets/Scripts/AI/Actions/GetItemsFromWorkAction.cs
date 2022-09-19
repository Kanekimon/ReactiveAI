using System.Collections.Generic;


public class GetItemsFromWorkAction : ActionBase
{
    Item item;
    int amount;
    InventorySystem workPlaceInventory;

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("hasItem", true));
        base.Start();
        workPlaceInventory = Agent.Job.Workplace.GetComponent<InventorySystem>() ?? null;
    }


    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        NavAgent.MoveTo(NavAgent.PickClosestPositionInRange(Agent.Job.Workplace, Agent.InteractionRange));
        amount = WorldState.GetValue<int>("gatherAmount");
        item = WorldState.GetValue<Item>("requestedItem");
    }

    public override void OnTick()
    {
        if (workPlaceInventory == null)
        {
            OnDeactived();
            return;
        }

        if (NavAgent.AtDestination)
        {
            if (workPlaceInventory.HasItemWithAmount(item, amount))
            {
                workPlaceInventory.TransferItemToOtherInventory(Agent.InventorySystem, item, amount);
                OnDeactived();
            }
            else
            {
                LinkedGoal.PauseGoal();
                NeedsReplanning();
            }
        }
    }


}

