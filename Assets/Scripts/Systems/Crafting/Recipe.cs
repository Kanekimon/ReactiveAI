using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class RecipesMaterial
{
    public Item Item;
    public int Amount;
}


[CreateAssetMenu(menuName = "Assets/Recipe")]
public class Recipe : ScriptableObject
{
    public List<RecipesMaterial> Materials = new List<RecipesMaterial>();
    public Item Result;
    public int Amount;
}

