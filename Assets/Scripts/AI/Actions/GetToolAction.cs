using System.Collections.Generic;
using System.Linq;


public class GetToolAction : ActionBase
{
    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("isAtWork", true));
        effects.Add(new KeyValuePair<string, object>("hasTool", true));
        base.Start();
    }

    public override void OnTick()
    {
        base.OnTick();

        InventorySystem workInventory = Agent.Job.Workplace.gameObject.GetComponent<InventorySystem>();
        foreach (Item i in Agent.Job.Tools)
        {
            if (workInventory.HasItem(i))
            {
                workInventory.TransferItemToOtherInventory(Agent.InventorySystem, i, 1);
                OnDeactived();
                return;
            }
        }

        List<Request> requests = Agent.WorldState.GetValue<List<Request>>("requests") ?? new List<Request>();
        requests.Add(new Request(Agent.gameObject, Agent.Job.Tools.FirstOrDefault(), 1));
        Agent.WorldState.AddWorldState("requests", requests);
    }


}

