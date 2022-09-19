using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Item")]
public class Item : ScriptableObject
{
    public int Id;
    public string Name;
    public List<ItemType> ItemTypes = new List<ItemType>();
    public bool IsResource;
    public bool HasRecipe;
    public GameObject Prefab;
    public List<ResourceType> ResourceType = new List<ResourceType>();
    public Sprite Icon;
    public float Weigth;
    public List<ItemProperty> Properties = new List<ItemProperty>();
}

