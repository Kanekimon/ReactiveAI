using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class JobManager : MonoBehaviour
{
    public static JobManager Instance;
    public List<Job> AllJobs = new List<Job>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    public void AddJobGoalsAndActions(Agent agent, Job job)
    {
        Transform aT = agent.gameObject.transform;
        string jobname = job.JobType.ToString();

        GameObject goalContainer = agent.transform.Find("Goals").gameObject;
        GameObject g = Instantiate(job.Goals, goalContainer.transform);
        g.name = $"{jobname}_Goals";

        GameObject actionContainer = agent.transform.Find("Actions").gameObject;
        GameObject a = Instantiate(job.Actions, actionContainer.transform);
        a.name = $"{jobname}_Actions";
    }

    public void RemoveJobGoalsAndActions(Agent agent, Job job)
    {
        Transform aT = agent.gameObject.transform;
        string jobname = job.JobType.ToString();

        GameObject goalContainer = agent.transform.Find("Goals").gameObject;
        GameObject goalToRemove = goalContainer.transform.Find($"{jobname}_Goals").gameObject;
        if (goalToRemove != null)
            DestroyImmediate(goalToRemove);

        GameObject actionContainer = agent.transform.Find("Actions").gameObject;
        GameObject actionToRemove = actionContainer.transform.Find($"{jobname}_Actions").gameObject;
        if (actionToRemove != null)
            DestroyImmediate(actionToRemove);

    }


}

