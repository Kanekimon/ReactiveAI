using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TownSystem : MonoBehaviour
{
    List<Agent> _population = new List<Agent>();
    public RequestSystem RequestBoard;

    public List<Storage> Storages = new List<Storage>();

    public void RegisterAgent(Agent agent)
    {
        _population.Add(agent);
    }

    public void DeregisterAgent(Agent agent)
    {
        _population.Remove(agent);
    }

    public bool RequestResoruces(Request r)
    {

        JobType requiredWorker = this.GetComponent<ResponsibilityChecker>().GetResponsibleJob(r.RequestedItem);
        List<Agent> workers = _population.Where(a => a.JobType == requiredWorker).ToList();

        Agent worker = null;

        if (workers.Count > 0)
        {
            //TODO: Check if occupied
            int random = UnityEngine.Random.Range(0, workers.Count);
            worker = workers[random];
        }
        else
        {
            if (_population.Any(a => a.JobType == JobType.None))
            {
                worker = _population.Where(a => a.JobType == JobType.None && a.gameObject != r.Requester).FirstOrDefault();
                if (worker == null)
                    return false;
                worker.GetJob(requiredWorker);
            }
        }

        if (worker == null)
        {
            return false;
        }
        else
        {
            JobManager.Instance.AddJobGoals(worker, requiredWorker);
            worker.GetComponent<GoapPlanner>().InitGoals();
            worker.AddWork(r.RequestedItem, r.RequestedAmount);
            r.Giver = worker.gameObject;
            return true;
        }
    }

    internal GameObject GetStorage(Item item)
    {
        return Storages.Where(a => a.CanStoreItem(item)).FirstOrDefault()?.gameObject ?? null;
    }

    public void FinishedRequest(Agent agent, Item itemDelivered, GameObject storage)
    {
        RequestBoard.GetComponent<RequestSystem>().FinishedRequest(agent, itemDelivered, storage);
    }
}

