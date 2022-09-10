using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TownSystem : MonoBehaviour
{
    List<Agent> _population = new List<Agent>();
    

    public RequestSystem RequestBoard;
    public List<Job> AvailableJobs = new List<Job>();
    public List<Storage> Storages = new List<Storage>();

    public GameObject TestWorkPlace;
    public Agent TestAgent;


    public GameObject GoblinPrefab;




    private void Start()
    {
        foreach(Job j in JobManager.Instance.AllJobs)
        {
            Job copy = Instantiate(j);
            copy.name = j.name;
            AddNewJob(copy, TestWorkPlace);
        }    
    }




    public void RegisterAgent(Agent agent)
    {
        _population.Add(agent);
    }

    public void DeregisterAgent(Agent agent)
    {
        _population.Remove(agent);
    }



    public void AddNewJob(Job job, GameObject workplace)
    {
        AvailableJobs.Add(job);
        job.Workplace = workplace;
    }
    

    public bool RequestResoruces(Request r)
    {
        Job job = AvailableJobs.Where(a => a.JobType == this.GetComponent<ResponsibilityChecker>().GetResponsibleJob(r.RequestedItem)).FirstOrDefault();
        JobType requiredWorker = job.JobType;
        List<Agent> workers = _population.Where(a => a.Job != null && a.Job.JobType == requiredWorker && !a.IsOccupied).ToList() ?? null;

        Agent worker = null;

        if (workers.Count > 0)
        {
            //TODO: Check if occupied
            int random = UnityEngine.Random.Range(0, workers.Count);
            worker = workers[random];
        }
        else
        {
            if (_population.Any(a => a.Job == null))
            {
                worker = _population.Where(a => a.Job == null && a.gameObject != r.Requester).FirstOrDefault();
                if (worker == null)
                    return false;

                HireWorker(worker, job);
            }
        }

        if (worker == null)
        {
            return false;
        }
        else
        {
            worker.AddWork(r);
            r.Giver = worker.gameObject;
            return true;
        }
    }

    public void HireWorker(Agent agent, Job job)
    {
        agent.GetJob(job);
        JobManager.Instance.AddJobGoalsAndActions(agent, job);
        agent.GetComponent<GoapPlanner>().InitGoals();
        agent.GetComponent<GoapPlanner>().InitActions();
    }

    public void FireWorker(Agent agent, Job job)
    {
        agent.GetJob(null);
        JobManager.Instance.RemoveJobGoalsAndActions(agent, job);
        agent.GetComponent<GoapPlanner>().InitGoals();
        agent.GetComponent<GoapPlanner>().InitActions();
    }

    internal GameObject GetStorage(Item item)
    {
        return Storages.Where(a => a.CanStoreItem(item)).FirstOrDefault()?.gameObject ?? null;
    }

    public void FinishedRequest(Agent agent, Item itemDelivered, GameObject storage)
    {
        RequestBoard.GetComponent<RequestSystem>().FinishedRequest(agent, itemDelivered, storage);
    }

    public GameObject SpawnAgent()
    {
        GameObject ag = Instantiate<GameObject>(GoblinPrefab);
        ag.GetComponent<Agent>().HomeTown = this;
        RegisterAgent(ag.GetComponent<Agent>());
        return ag;
    }
}

