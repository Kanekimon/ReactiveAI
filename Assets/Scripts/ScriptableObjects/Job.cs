using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Job")]
public class Job : ScriptableObject
{
    public GameObject Workplace;
    public JobType JobType;
    public List<ResourceType> GatherThese = new List<ResourceType>();
    public List<Item> Tools = new List<Item>();
    public GameObject Actions;
    public GameObject Goals;
    public string WorkAnimState;

}

