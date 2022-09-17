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
    loot,
    none
}

[Serializable]
public class Resource
{
    public Item Item;
    public int MinAmount;
    public int MaxAmount;
    public int Amount;
    public float Propability = 1f;
}


public class ResourceTarget : DetectableTarget
{
    [SerializeField] public List<Resource> GatherableMaterials = new List<Resource>();
    public Resource Primary;
    public ResourceType ResourceType;

    protected override void Start()
    {
        base.Start();
        Primary = GatherableMaterials[0];
    }


    public List<InventoryItem> Interact(InventorySystem inventory)
    {
        List<InventoryItem> gatheredItems = new List<InventoryItem>();

        int gatheredAmount = 0;
        for (int i = GatherableMaterials.Count - 1; i >= 0; i--)
        {
            if(GatherableMaterials[i].Propability < 1f)
            {
                if (Random.Range(0f, 1f) > GatherableMaterials[i].Propability)
                    continue;
            }

            Resource res = GatherableMaterials[i];
            gatheredAmount = Mathf.Clamp(Random.Range(res.MinAmount, res.MaxAmount + 1), 1, res.Amount);
            gatheredAmount = inventory.AddItem(res.Item, gatheredAmount);
            res.Amount -= gatheredAmount;
            gatheredItems.Add(new InventoryItem() { Item = res.Item, Amount = gatheredAmount });

            if (res.Amount == 0)
                GatherableMaterials.RemoveAt(i);
        }

        if (!GatherableMaterials.Contains(Primary))
            Destroy(this.gameObject);
        return gatheredItems;
    }

}

