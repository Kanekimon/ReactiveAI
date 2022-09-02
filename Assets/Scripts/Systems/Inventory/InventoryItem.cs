using System;




[Serializable]
public class InventoryItem
{
    public string Name { get; set; }

    public Item Item;

    public int Amount;

    public void Add(int amount)
    {
        Amount += amount;
    }

    public void Remove(int amount)
    {
        Amount -= amount;
    }

}
