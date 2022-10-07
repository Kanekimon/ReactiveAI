using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;


[Serializable]
public class ResourceData
{
    public GameObject Prefab;

    [Range(0f, 1f)] 
    public float Probability;
}

public class ResourceSpawner : MonoBehaviour
{
    public List<ResourceData> ResourcePrefabs = new List<ResourceData>();
    public int ResourceCount;
    Vector3 Center;
    public float Radius;

    List<GameObject> spawned = new List<GameObject>();

    // Start is called before the first frame update
    void Start()
    {


        //SpawnResources();

    }

    public void SpawnResources()
    {
           Center = transform.position;
        int counter = 0;
        int endlessLimiter = 0;
        while (counter < ResourceCount && endlessLimiter < 100)
        {
            int randomIndex = Random.Range(0, ResourcePrefabs.Count);

            if (ResourcePrefabs[randomIndex].Probability < Random.Range(0f, 1f))
            {
                endlessLimiter++;
                continue;
            }

            float x = Random.Range(Center.x - Radius, Center.x + Radius);
            float z = Random.Range(Center.z - Radius, Center.z + Radius);
            Vector3 spawnPoint = new Vector3(x, 0, z);
            float y = Terrain.activeTerrain.SampleHeight(spawnPoint);
            spawnPoint.y = y;

            if (!ValidPosition(spawnPoint))
            {
                endlessLimiter++;
                continue;
            }


            GameObject resource = Instantiate(ResourcePrefabs[randomIndex].Prefab);
            resource.transform.position = spawnPoint;
            resource.transform.parent = this.gameObject.transform;
            RotateRandom(resource);
            spawned.Add(resource);
            counter++;
            endlessLimiter = 0;
        }
    }

    public void SpawnResourcesFromEditor()
    {
        Center = transform.position;
        int counter = 0;
        int endlessLimiter = 0;
        while (counter < ResourceCount && endlessLimiter < 100)
        {
            int randomIndex = Random.Range(0, ResourcePrefabs.Count);

            if (ResourcePrefabs[randomIndex].Probability < Random.Range(0f, 1f))
            {
                endlessLimiter++;
                continue;
            }

            float x = Random.Range(Center.x - Radius, Center.x + Radius);
            float z = Random.Range(Center.z - Radius, Center.z + Radius);
            Vector3 spawnPoint = new Vector3(x, 0, z);
            float y = Terrain.activeTerrain.SampleHeight(spawnPoint);
            spawnPoint.y = y;

            if (!ValidPosition(spawnPoint))
            {
                endlessLimiter++;
                continue;
            }

  
            GameObject resource = PrefabUtility.InstantiatePrefab(ResourcePrefabs[randomIndex].Prefab) as GameObject;
            resource.transform.position = spawnPoint;
            resource.transform.parent = this.gameObject.transform;
            RotateRandom(resource);
            spawned.Add(resource);
            counter++;
            endlessLimiter = 0;
        }
    }

    public void RemoveAll()
    {
        foreach(GameObject r in spawned)
        {
            DestroyImmediate(r);
        }
        spawned.Clear();
    }

    void RotateRandom(GameObject toRotate)
    {
        Vector3 euler = toRotate.transform.eulerAngles;
        euler.y = Random.Range(0f, 360f);
        toRotate.transform.eulerAngles = euler;
    }

    bool ValidPosition(Vector3 org)
    {
        if (!Terrain.activeTerrain.terrainData.bounds.Contains(org))
            return false;

        int[] maskX = new int[] { -5, 0, 5, 5, 5, 0, -5, -5 };
        int[] maskZ = new int[] { 5, 5, 5, 0, -5, -5, -5, 0 };


       for(int i = 0; i < maskX.Length; i++)
        {
            Vector3 probe = new Vector3(org.x + maskX[i], org.y, org.z + maskZ[i]);
            float diff = Mathf.Abs(Terrain.activeTerrain.SampleHeight(probe) - org.y);
            if (diff > 5)
                return false;
        }

        return true;
    }



}
