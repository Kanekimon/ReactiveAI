using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    private List<Item> _allItems = new List<Item>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {
        _allItems.Add(new Item() { Id = 0, Name = "wood", ItemTypes = new List<ItemType>() { ItemType.Resource }, IsResource = true });
        _allItems.Add( new Item() { Id = 1, Name = "stone", ItemTypes = new List<ItemType>() { ItemType.Resource }, IsResource = true });
        _allItems.Add( new Item() { Id = 1, Name = "food", ItemTypes = new List<ItemType>() { ItemType.Resource, ItemType.Food }, IsResource = true });
    }

    public Item GetItemByName(string name)
    {
        return _allItems.Where(a => a.Name.Equals(name)).FirstOrDefault();
    }

    public string GetItemFromResourceType(ResourceType resType)
    {
        switch (resType)
        {
            case ResourceType.Stone:
                return "stone";
            case ResourceType.Wood:
                return "wood";
            case ResourceType.Food:
                return "food";
            default:
                return "";
        }
    }




}

