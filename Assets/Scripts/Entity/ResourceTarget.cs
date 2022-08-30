using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public enum ResourceType
{
    rock, 
    tree,
    bush,
    mushroom,
    none
}

[Serializable]
public class Resource
{
    public Item Item;
    public int MinAmount;
    public int MaxAmount;
    public int Amount;
}


public class ResourceTarget : DetectableTarget
{
    [SerializeField] public List<Resource> GatherableMaterials = new List<Resource>();
    public ResourceType ResourceType;

    protected override void Start()
    {
        base.Start();
    }


    public void Interact(Agent interacted)
    {
        for(int i = GatherableMaterials.Count - 1; i >= 0; i--)
        {
            Resource res = GatherableMaterials[i];
            int gatheredAmount = Mathf.Clamp(Random.Range(res.MinAmount, res.MaxAmount + 1), 1, res.Amount);

            res.Amount -= gatheredAmount;
            interacted.InventorySystem.AddItem(res.Item, gatheredAmount);

            if (res.Amount == 0)
                GatherableMaterials.RemoveAt(i);
        }

        if (GatherableMaterials.Count == 0)
            Destroy(this.gameObject);

    }

}

