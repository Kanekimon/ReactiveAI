using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;


[Serializable]
public class JobResponsibilities
{
    public JobType JobType;
    public List<Item> Gathering = new List<Item>();
}


public class ResponsibilityChecker : MonoBehaviour
{
    public List<JobResponsibilities> Responsibilities = new List<JobResponsibilities>();

    public JobType GetResponsibleJob(Item item)
    {
        foreach(JobResponsibilities resp in Responsibilities)
        {
            if (resp.Gathering.Contains(item))
                return resp.JobType;
        }

        return JobType.None;
    }

}

