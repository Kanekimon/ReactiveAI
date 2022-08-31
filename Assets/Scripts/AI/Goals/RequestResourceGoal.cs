using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class RequestResourceGoal : BaseGoal
{
    [SerializeField] int Priority = 10;
    [SerializeField] int MinPrio = 1;
    [SerializeField] int MaxPrio = 100;

    [SerializeField] List<Request> Requests = new List<Request>();
    public bool waiting;



    protected override void Start()
    {
        Preconditions.Add(new KeyValuePair<string, object>("requestResource", true));

        if (Agent.name.Equals("Agent#02"))
        {
            Requests.Add(new Request(Agent.gameObject, ItemManager.Instance.GetItemByName("hammer"), 1));
            Agent.WorldState.AddWorldState("requests", Requests);
        }

       
        base.Start();
    }

    public override void OnGoalActivated()
    {
        Agent.WorldState.AddWorldState("target", Agent.HomeTown.RequestBoard.gameObject);
        base.OnGoalActivated();
    }

    public override int CalculatePriority()
    {
        return Priority;
    }

    public override void OnGoalDeactivated()
    {
        if (Agent.JobType == JobType.Crafter)
            Debug.Log("Crafter");

        Requests.Clear();
        Agent.WorldState.AddWorldState("deliveredResource", false);
        Agent.WorldState.AddWorldState("requests", null);
        base.OnGoalDeactivated();
    }

    public override void OnTickGoal()
    {
        if (Agent.JobType == JobType.Crafter)
            Debug.Log("Crafter");

        if (Agent.WorldState.GetValue("requests") != null)
        {
            Requests = Agent.WorldState.GetValue("requests") as List<Request>;

            if (Requests.Count > 0)
            {
                Priority = MaxPrio;//waiting ? (MinPrio + MaxPrio)/2 : MaxPrio;
            }
            else
            {
                Priority = MinPrio;
            }
        }
        else
        {          
            Priority = MinPrio;
        }
    }


    public void AddRequests(List<Request> r)
    {
        Requests = r;
    }
}

