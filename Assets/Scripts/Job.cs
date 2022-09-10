using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[CreateAssetMenu(menuName ="Job")]
public class Job : ScriptableObject
{
    public GameObject Workplace;
    public JobType JobType;
    public List<ResourceType> GatherThese = new List<ResourceType>();
    public List<Item> Tools = new List<Item>();
    public GameObject Actions;
    public GameObject Goals;
}

