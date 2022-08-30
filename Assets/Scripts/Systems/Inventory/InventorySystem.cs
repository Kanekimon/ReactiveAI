using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public Dictionary<Item, InventoryItem> _inventory = new Dictionary<Item, InventoryItem>();

    /// <summary>
    /// Add Items with amount
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <param name="amount">Amount to add</param>
    public void AddItem(Item item, int amount)
    {
        if (_inventory.Any(a => a.Key.Id == item.Id))
        {
            InventoryItem i = _inventory.Where(a => a.Key.Id == item.Id).FirstOrDefault().Value;
            i.Add(amount);
        }
        else
            _inventory.Add(item, new InventoryItem() { Item = item, Amount = amount, Name = item.Name});
    }

    /// <summary>
    /// Removes specific amount of an item from inventory
    /// If quantity is 0 afterwards, the item gets removed entirely
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public void RemoveItem(Item item, int amount)
    {
        if (HasEnough(item, amount))
        {
            InventoryItem i = _inventory.Where(a => a.Key.Id == item.Id).FirstOrDefault().Value;
            i.Remove(amount);

            if (i.Amount == 0)
                _inventory.Remove(item);
        }
    }

    internal void TransferToOther(InventorySystem inventorySystem, Item toDeliver, int amount)
    {
        if(HasEnough(toDeliver, amount))
        {
            inventorySystem.AddItem(toDeliver, amount);
            this.RemoveItem(toDeliver, amount);
        }
    }


    /// <summary>
    /// Checks if the item is in the inventory
    /// and its amount is equal or greater than requested
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool HasEnough(Item item, int amount)
    {
        return HasItem(item) && (_inventory.Where(a => a.Key.Id == item.Id).FirstOrDefault().Value.Amount >= amount);
    }

    /// <summary>
    /// Checks if the item is in the inventory
    /// and its amount is equal or greater than requested
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool HasEnough(string item_name, int amount)
    {
        return HasItem(item_name) && (_inventory.Where(a => a.Key.Name == item_name).FirstOrDefault().Value.Amount >= amount);
    }

    /// <summary>
    /// Checks if an item is in the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasItem(Item item)
    {
        return _inventory.Any(a => a.Key.Id == item.Id);
    }

    /// <summary>
    /// Checks if an item is in the inventory
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasItem(string item_name)
    {
        return _inventory.Any(a => a.Key.Name.Equals(item_name));
    }

    /// <summary>
    /// Checks if inventory contains items of specifiec type
    /// </summary>
    /// <param name="type"></param>
    /// <returns></returns>
    public bool HasItemWithType(ItemType type)
    {
        return _inventory.Any(a => a.Key.ItemTypes.Contains(type));
    }

    public Item GetFirstItemWithType(ItemType type)
    {
        return _inventory.Where(a => a.Key.ItemTypes.Contains(type)).FirstOrDefault().Key;
    }


}

