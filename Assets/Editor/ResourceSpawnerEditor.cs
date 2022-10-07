using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceSpawner))]
public class ResourceSpawnerEditor : Editor
{

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        ResourceSpawner spawner = (ResourceSpawner)target;

        if (GUILayout.Button("Generate"))
        {
            Reset(spawner);
            spawner.SpawnResourcesFromEditor();
        }

        if (GUILayout.Button("Reset"))
        {
            Reset(spawner);

        }


    }

    private void Reset(ResourceSpawner spawner)
    {

        var tempArray = new GameObject[spawner.transform.childCount];

        for (int i = 0; i < tempArray.Length; i++)
        {
            tempArray[i] = spawner.transform.GetChild(i).gameObject;
        }

        foreach (var child in tempArray)
        {
            DestroyImmediate(child);
        }
    }

}

