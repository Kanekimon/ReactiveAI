using System.Collections.Generic;


public class RequestAtTownAction : ActionBase
{
    TownSystem town;

    protected override void Awake()
    {
        base.Awake();
        town = Agent.GetComponent<TownSystem>();
    }

    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("requestResource", true));
        preconditions.Add(new KeyValuePair<string, object>("isAtTarget", true));

        base.Start();
    }

    public override void OnTick()
    {
        base.OnTick();

        List<Request> Requests = WorldState.GetValue<List<Request>>("requests");
        Agent.HomeTown.RequestBoard.RequestItems(Agent, Requests);
        //Agent.HomeTown.RequestBoard.GetComponent<RequestSystem>().RequestItem(Agent, WorldState.GetValue("requestItem") as Item, int.Parse(WorldState.GetValue("requestAmount").ToString()));
        WorldState.AddWorldState("requestResource", true);
        OnDeactived();
    }

}
