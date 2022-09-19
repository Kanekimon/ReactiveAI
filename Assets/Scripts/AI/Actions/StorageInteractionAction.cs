using System.Collections.Generic;

internal class StorageInteractionAction : ActionBase
{
    Request request;
    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("isAtPosition", true));
        effects.Add(new KeyValuePair<string, object>("interactWithStorage", true));

        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        request = WorldState.GetValue<Request>("request");
    }

    public override void OnTick()
    {
        base.OnTick();
        request = WorldState.GetValue<Request>("request");
        if (request.Storage.GetComponent<InventorySystem>().TransferItemToOtherInventory(Agent.InventorySystem, request.RequestedItem, request.RequestedAmount))
        {
            request.Status = RequestStatus.Finished;
            Agent.ReadyForPickUp.Dequeue();
            OnDeactived();
        }
    }

}

