using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

[Serializable]
public class Job
{
    public Storage Workplace;
    public JobType JobType;
    public List<ResourceType> GatherThese = new List<ResourceType>();  
}

