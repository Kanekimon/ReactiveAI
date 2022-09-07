﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class InventorySystem : MonoBehaviour
{
    public Dictionary<int, InventoryItem> _inventory = new Dictionary<int, InventoryItem>();

    [SerializeField] List<InventoryItem> _items = new List<InventoryItem>();
    public List<InventoryItem> InventoryItems { get => _items; set { _items = value; } }
    public int MaximumStackSize;
    public int MaximumSlots;
    public int NumberOfItems => _inventory.Values.Sum(a => a.Amount);

    private void Start()
    {
        for (int i = 0; i < MaximumSlots; i++)
        {
            _inventory[i] = null;
        }
    }

    private void Update()
    {
        InventoryItems = _inventory.Values.ToList();
    }

    /// <summary>
    /// Checks if inventory contains item
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public bool HasItem(Item item)
    {
        return _inventory.Any(a => a.Value != null && a.Value.Item.Id == item.Id);
    }

    /// <summary>
    /// Checks if there is a stack of an item in the inventory with space 
    /// </summary>
    /// <param name="item">Item to check for</param>
    /// <returns>index of stack with free space 
    /// if there is none, then the index of the next free slot
    /// If no free slot is available it returns -1
    /// </returns>
    public int GetInventoryItemWithSpace(Item item)
    {
        return _inventory.Where(a => a.Value != null && a.Value.Item.Id == item.Id && a.Value.Amount < MaximumStackSize)?.FirstOrDefault().Key ?? GetNextFreeSlot();
    }

    /// <summary>
    /// Search for the index of the next free slot in the inventory
    /// </summary>
    /// <returns>Index for the next free slot or -1 if no slot is empty</returns>
    public int GetNextFreeSlot()
    {
        return _inventory.Where(a => a.Value == null)?.FirstOrDefault().Key ?? -1;
    }


    /// <summary>
    /// Adds an item to the inventory
    /// If Item already in inventory an stack is lower than stacksize, amount will be added
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <param name="amount">Amoun to add</param>
    /// <returns>The amount that is added (might not be equal to the given amount because there is no space)</returns>
    public int AddItem(Item item, int amount)
    {
        int added = 0;
        if (HasItem(item))
        {
            int slotIndex = GetInventoryItemWithSpace(item);
            if (slotIndex != -1)
            {
                if (_inventory[slotIndex] != null)
                {
                    int invStackSize = _inventory[slotIndex].Amount;
                    if (invStackSize + amount <= MaximumStackSize)
                    {
                        _inventory[slotIndex].Amount += amount;
                        added += amount;
                    }
                    else
                    {
                        int restAmount = (invStackSize + amount) - MaximumStackSize;
                        _inventory[slotIndex].Amount += (amount - restAmount);
                        added += CreateNewSlots(item, restAmount);
                    }
                }
                else
                {
                    added += CreateNewSlots(item, amount);
                }
            }
        }
        else
        {
            added += CreateNewSlots(item, amount);
        }
        return added;
    }

    /// <summary>
    /// Addes item to new slots
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <param name="amount">Amount of item</param>
    /// <returns>The amount of the item that was added</returns>
    public int CreateNewSlots(Item item, int amount)
    {
        int added = 0;
        if (amount > MaximumStackSize)
        {
            while (amount > 0)
            {
                int adding = amount > MaximumStackSize ? MaximumStackSize : amount;
                if (AddNewItem(item, adding))
                {
                    amount -= adding;
                    added += adding;
                }
            }
        }
        else
        {
            if (AddNewItem(item, amount))
                added += amount;
        }
        return added;
    }

    /// <summary>
    /// Returns InventoryItem at given slot index
    /// </summary>
    /// <param name="index">index of the slot</param>
    /// <returns>InventoryItem at Index</returns>
    public InventoryItem GetItemAtSlot(int index)
    {
        return _inventory[index];
    }

    /// <summary>
    /// Adds a new item to the inventory system if there is space
    /// </summary>
    /// <param name="item">Item to add</param>
    /// <param name="amount">Amount of the item</param>
    /// <returns>true if item could be added, false otherwise</returns>
    public bool AddNewItem(Item item, int amount)
    {
        int s_index = GetNextFreeSlot();
        if (s_index != -1)
        {
            _inventory[s_index] = new InventoryItem() { Item = item, Amount = amount };
            return true;
        }
        return false;
    }


    /// <summary>
    /// Checks if the stack of an inventory item is full
    /// </summary>
    /// <param name="inv"></param>
    /// <returns></returns>
    public bool IsStackFull(InventoryItem inv)
    {
        return inv.Amount == MaximumStackSize;
    }

    public bool HasItemWithAmount(Item item, int amount)
    {
        foreach (InventoryItem i in _inventory.Values.Where(a => a != null))
        {
            if (i.Item.Id == item.Id)
                amount -= i.Amount;

            if (amount <= 0)
                return true;
        }
        return false;
    }


    /// <summary>
    /// Removes a given amount of an item from the inventory
    /// </summary>
    /// <param name="item">Item to remove</param>
    /// <param name="amount">Amount to remove</param>
    /// <returns>True if amount of items are removed, false if there are not enough items</returns>
    public bool RemoveItem(Item item, int amount)
    {
        if (!HasItemWithAmount(item, amount))
            return false;

        List<InventoryItem> items = _inventory.Values.Where(a => a != null && a.Item.Id == item.Id).ToList();

        for (int i = items.Count - 1; i >= 0; i--)
        {
            if (items[i].Amount < amount)
            {
                amount -= items[i].Amount;
                _inventory[i] = null;
            }
            else
            {
                _inventory[i].Amount -= amount;
                return true;
            }

        }
        return false;
    }


    /// <summary>
    /// Transfers a item with an amount to another inventory
    /// and removes it from this inventory
    /// </summary>
    /// <param name="otherInventory">Transfer to this</param>
    /// <param name="toDeliver">item to transfer</param>
    /// <param name="amount">amount to transfer</param>
    public void TransferItemToOtherInventory(InventorySystem otherInventory, Item toDeliver, int amount)
    {
        if(HasItemWithAmount(toDeliver, amount))
        {
            int added = otherInventory.AddItem(toDeliver, amount);
            this.RemoveItem(toDeliver, added);
        }
    }

    /// <summary>
    /// Returns how many of given item is in the inventory
    /// </summary>
    /// <param name="item">Item</param>
    /// <returns>Quantity of item</returns>
    public int GetItemAmount(Item item)
    {
        if (!HasItem(item))
            return 0;

        int amount = 0;

        foreach(InventoryItem inv_item in _inventory.Values.Where(a => a != null && a.Item.Id == item.Id)) 
        { 
            amount += inv_item.Amount;  
        }

        return amount;
    }

    /// <summary>
    /// Returns the amount of free slots in inventory
    /// </summary>
    /// <returns>Number of free slots</returns>
    public int GetNumberOfFreeSlots()
    {
        return _inventory.Count(a => a.Value == null);
    }

    /// <summary>
    /// Checks if an item with a given type is in the inventory
    /// </summary>
    /// <param name="itemType">Type to check for</param>
    /// <returns>True if has itemtype, false otherwise</returns>
    public bool HasItemWithType(ItemType itemType)
    {
        return _inventory.Any(a => a.Value != null && a.Value.Item.ItemTypes.Contains(itemType));
    }

    /// <summary>
    /// Returns the first item with a type from the inventory
    /// </summary>
    /// <param name="type">Type</param>
    /// <returns>First item of type</returns>
    public Item GetFirstItemWithType(ItemType type)
    {
        return _inventory.Where(a => a.Value.Item.ItemTypes.Contains(type)).FirstOrDefault().Value.Item;
    }

}





