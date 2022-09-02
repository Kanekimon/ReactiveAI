using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Storage : MonoBehaviour
{
    public List<ItemType> StorableTypes = new List<ItemType>();
    public InventorySystem InventorySystem;

    private void Awake()
    {
    }


    public bool CanStoreItem(Item item)
    {
        if (this.StorableTypes.Any(a => item.ItemTypes.Contains(a)))
            return true;

        return false;
    }

    public void PutIntoStorage(Item item, int amount)
    {
        InventorySystem.AddItem(item, amount);
    }

    public void RetrieveFromStorage(InventorySystem retriever, Item item, int amount)
    {
        InventorySystem.TransferToOther(retriever, item, amount);
    }
}

