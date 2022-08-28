using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class TownSystem : MonoBehaviour
{
    List<Agent> _population = new List<Agent>();

    public GameObject WoodStorage;
    public GameObject StoneStorage;
    public GameObject FoodStorage;
    public GameObject GeneralStorage;
    public GameObject RequestBoard;

    public void RegisterAgent(Agent agent)
    {
        _population.Add(agent);
    }

    public void DeregisterAgent(Agent agent)
    {
        _population.Remove(agent);
    }

    public void RequestResoruces(ResourceType resource, int amount)
    {
        List<Agent> worker = _population.Where(a => a.JobType == JobType.Woodcutter).ToList();
        if(worker.Count > 0)
        {
            //TODO: Check if occupied
            int random = UnityEngine.Random.Range(0, worker.Count);
            worker[random].GetRequest(resource, amount);
        }
        else
        {
            if(_population.Any(a => a.JobType == JobType.None))
            {
                Agent newWorker = _population.Where(a => a.JobType == JobType.None).FirstOrDefault();
                newWorker.GetJob(JobType.Woodcutter);
                newWorker.GetRequest(resource, amount);
            }
        }
    }




    internal GameObject GetStorage(ResourceType resource)
    {
        switch (resource)
        {
            case ResourceType.Stone:
                return StoneStorage;
            case ResourceType.Wood:
                return WoodStorage;
            case ResourceType.Food:
                return FoodStorage;
            default:
                return GeneralStorage;
        }
    }
}

