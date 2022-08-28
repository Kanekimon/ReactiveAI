using UnityEngine;

public enum ResourceType
{
    Stone,
    Wood,
    Food
}

public class ResourceTarget : DetectableTarget
{
    public ResourceType ResourceType;
    public int MinAmount;
    public int MaxAmount;
    public int Amount;
    public string itemName;




    protected override void Start()
    {
        base.Start();
    }


    public void Interact(Agent interacted)
    {
        int gatheredAmount = Mathf.Clamp(Random.Range(MinAmount, MaxAmount + 1), 1, Amount);

        Amount -= gatheredAmount;
        interacted.InventorySystem.AddItem(ItemManager.Instance.GetItemByName(itemName), gatheredAmount);

        if (Amount == 0)
            Destroy(this.gameObject);
    }

}

