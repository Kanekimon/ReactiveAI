using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class TownSystem : MonoBehaviour
{
    List<Agent> _population = new List<Agent>();

    public List<Agent> Villager => _population;

    public RequestSystem RequestBoard;
    public List<Job> AvailableJobs = new List<Job>();

    public GameObject TestWorkPlace;
    public Agent TestAgent;

    public List<GameObject> Homes = new List<GameObject>();
    public List<GameObject> Workplaces = new List<GameObject>();

    public GameObject GoblinPrefab;

    private int _playerReputation;


    private void Start()
    {
        Homes = GameObject.FindGameObjectsWithTag("AgentHouse").ToList();
        Workplaces = GameObject.FindGameObjectsWithTag("Workplace").ToList();

        //foreach (Job j in JobManager.Instance.AllJobs)
        //{
        //    Job copy = Instantiate(j);
        //    copy.name = j.name;
        //    AddNewJob(copy, TestWorkPlace);
        //}
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

        if (job == null)
            return false;

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

    public void FireWorker(Agent agent)
    {
        JobManager.Instance.RemoveJobGoalsAndActions(agent, agent.Job);
        agent.GetComponent<GoapPlanner>().InitGoals();
        agent.GetComponent<GoapPlanner>().InitActions();
        agent.GetJob(null);
    }


    public void FinishedRequest(Agent agent, Item itemDelivered, GameObject storage)
    {
        RequestBoard.GetComponent<RequestSystem>().FinishedRequest(agent, itemDelivered, storage);
    }

    public GameObject SpawnAgent()
    {
        GameObject ag = Instantiate<GameObject>(GoblinPrefab);
        ag.GetComponent<NavMeshAgent>().Warp(NavMeshHelper.GetPosititionOnNavMeshInRange(this.transform.position, 10f));
        ag.GetComponent<Agent>().HomeTown = this;
        ag.GetComponent<Agent>().Home = Homes[Random.Range(0, Homes.Count)];
        RegisterAgent(ag.GetComponent<Agent>());
        return ag;
    }
}

