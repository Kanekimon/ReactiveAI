using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class PlaceObjectAction : ActionBase
{
    Item toPlace;
    int AmountToPlace = 0;
    protected override void Start()
    {
        effects.Add(new KeyValuePair<string, object>("placedObject", true));
        base.Start();
    }

    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        toPlace = WorldState.GetValue<Item>("itemToPlace");
        AmountToPlace = WorldState.GetValue<int>("placeAmount");

    }

    public override void OnTick()
    {
        AmountToPlace = WorldState.GetValue<int>("placeAmount");
        /**
         *                        
         *    ----------------------  z2
         *   |                      |
         *   |                      |
         *   |                      |
         *   |                      |
         *   |         pos          |
         *   |                      |
         *   |                      |
         *   |                      |
         *   |                      |
         *    ----------------------- z1
         *   x1                    x2
        */
        for (int i = AmountToPlace - 1; i >= 0; i--)
        {
            GameObject placed = null;
            Vector3 randomPos = GetRandomPositionInRange(toPlace.Prefab.transform.localScale);

            if (randomPos != new Vector3(-1, -1, -1))
            {
                placed = Instantiate(toPlace.Prefab);
                placed.transform.localPosition = randomPos;
                Agent.InventorySystem.RemoveItem(toPlace, 1);
                WorldState.AddWorldState("placeAmount", i);
            }
        }

        if (WorldState.GetValue<int>("placeAmount") > 0)
        {
            LinkedGoal.PauseGoal();
            NeedsReplanning();
        }

        OnDeactived();
        //GameObject placed = Instantiate(toPlace.Prefab);

    }


    public Vector3 GetRandomPositionInRange(Vector3 size)
    {
        int counter = 10;
        while (counter > 0)
        {
            float randomX = Random.Range(Agent.transform.position.x + Agent.InteractionRange, Agent.transform.position.x - Agent.InteractionRange);
            float randomZ = Random.Range(Agent.transform.position.z + Agent.InteractionRange, Agent.transform.position.z - Agent.InteractionRange);
            float y = Terrain.activeTerrain.SampleHeight(new Vector3(randomX, 0, randomZ));

            Vector3 randomPoint = new Vector3(randomX, y, randomZ);
            Debug.DrawRay(randomPoint, Vector3.up * 10, Color.cyan, 100);

            float x1 = randomPoint.x - size.x / 2;
            float x2 = randomPoint.x + size.x / 2;
            float z1 = randomPoint.z - size.z / 2;
            float z2 = randomPoint.z + size.z / 2;

            randomPoint.y += size.y / 2;

            if (IsSpaceFree(randomPoint, size) &&
                IsValidEdge(new Vector3(x1, randomPoint.y, z1), size) &&
                IsValidEdge(new Vector3(x2, randomPoint.y, z1), size) &&
                IsValidEdge(new Vector3(x1, randomPoint.y, z2), size) &&
                IsValidEdge(new Vector3(x2, randomPoint.y, z2), size))
            {
                return randomPoint;
            }

            counter--;
        }

        return new Vector3(-1, -1, -1);
    }

    public bool IsSpaceFree(Vector3 pos, Vector3 size)
    {
        int layerMask = ~LayerMask.GetMask("Terrain");
        Vector3 halfSize = size / 2;
        Collider[] overlap = Physics.OverlapBox(pos, halfSize);
        if (overlap != null && overlap.Length > 0)
        {
            Debug.Log("Test");
        }

        if (Physics.CheckBox(pos, size / 2, Quaternion.identity, layerMask))
        {
            //Debug.DrawRay(pos, Vector3.up, Color.red, 100);
            return false;
        }

        return true;
    }

    public bool IsValidEdge(Vector3 pos, Vector3 size)
    {
        int layerMask = ~LayerMask.GetMask("Terrain");
        Vector3 posyzero = pos;
        posyzero.y = 0;
        float sH = Terrain.activeTerrain.SampleHeight(pos);
        bool inBounds = Terrain.activeTerrain.terrainData.bounds.Contains(posyzero);
        bool aboveTerrain = pos.y >= sH;
        bool notFloatin = pos.y <= (sH + size.y);

        if (inBounds && aboveTerrain && notFloatin)
        {
            //Debug.DrawRay(pos, Vector3.up, Color.green, 100);
            return true;
        }
        else
        {
            //Debug.DrawRay(pos, Vector3.up, Color.red, 100);
            return false;
        }
    }

}
