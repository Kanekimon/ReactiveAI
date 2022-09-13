using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(TownSystem))]
public class TownCustomEditor : Editor
{
    string amount;
    List<GameObject> agents = new List<GameObject>();
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TownSystem town = (TownSystem)target;

        if (GUILayout.Button("Hire"))
        {
            Job job = town.AvailableJobs.Where(a => a.JobType == JobType.Farmer).FirstOrDefault();
            town.HireWorker(town.TestAgent, job);
        }

        if (GUILayout.Button("Spawn"))
        {
            GameObject ran = town.SpawnAgent();
            Item pickaxe = ItemManager.Instance.GetItemByName("pickaxe");
            Item wood = ItemManager.Instance.GetItemByName("wood");
            ran.GetComponent<InventorySystem>().AddItem(pickaxe, 1);
            ran.GetComponent<InventorySystem>().AddItem(wood, 5);
            agents.Add(ran);

        }
    }

}

