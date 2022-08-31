using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


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

        List<Request> Requests = Agent.WorldState.GetValue("requests") as List<Request>;
        Agent.HomeTown.RequestBoard.RequestItems(Agent, Requests);
        //Agent.HomeTown.RequestBoard.GetComponent<RequestSystem>().RequestItem(Agent, Agent.WorldState.GetValue("requestItem") as Item, int.Parse(Agent.WorldState.GetValue("requestAmount").ToString()));
        Agent.WorldState.AddWorldState("requestResource", true);
        OnDeactived();
    }

}
