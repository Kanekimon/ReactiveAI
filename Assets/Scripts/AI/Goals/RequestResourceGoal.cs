using System.Collections.Generic;
using UnityEngine;

public class RequestResourceGoal : BaseGoal
{
    [SerializeField] List<Request> Requests = new List<Request>();
    public bool waiting;



    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("requestResource", true));

        //if (Agent.name.Equals("Agent#02"))
        //{
        //    Requests.Add(new Request(Agent.gameObject, ItemManager.Instance.GetItemByName("hammer"), 1));
        //    WorldState.AddWorldState("requests", Requests);
        //}


        base.Start();
    }

    public override void OnGoalActivated()
    {
        WorldState.AddWorldState("target", Agent.HomeTown.RequestBoard.gameObject);
        base.OnGoalActivated();
    }

    public override int CalculatePriority()
    {
        return Prio;
    }

    public override void OnGoalDeactivated()
    {
        Requests.Clear();
        WorldState.AddWorldState("target", null);
        WorldState.AddWorldState("deliveredResource", false);
        WorldState.AddWorldState("requests", null);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {


        if (WorldState.GetValue<List<Request>>("requests") != null)
        {
            Requests = WorldState.GetValue<List<Request>>("requests");

            if (Requests.Count > 0)
            {
                Prio = MaxPrio;
            }
            else
            {
                Prio = MinPrio;
            }
        }
        else
        {
            Prio = MinPrio;
        }
    }

}

