using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    public static JobManager Instance;

    public List<GameObject> JobGoals = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void AddJobGoals(Agent agent, JobType job)
    {
        Transform aT = agent.gameObject.transform;
        GameObject goalContainer = agent.gameObject.transform.Find("GoalContainer").gameObject;
        DestroyImmediate(goalContainer);

        string jobname = job.ToString();
        GameObject g = Instantiate(JobGoals.Where(a => a.name.ToLower().Equals(jobname.ToLower())).FirstOrDefault(), aT);
        g.name = "GoalContainer";
    }


}

