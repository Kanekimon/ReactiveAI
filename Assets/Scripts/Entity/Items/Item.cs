using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Assets/Item")]
public class Item : ScriptableObject
{
    public int Id;
    public string Name;
    public List<ItemType> ItemTypes = new List<ItemType>();
    public bool IsResource;
    public bool HasRecipe;
    public GameObject Prefab;
    public List<ResourceType> ResourceType = new List<ResourceType>();
}

