using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(InventorySystem))]
public class CraftingSystem : MonoBehaviour
{
    public List<Recipe> Learned = new List<Recipe>();
    InventorySystem inventorySystem;

    private void Start()
    {
        inventorySystem = GetComponent<InventorySystem>();
    }


    public bool CanCraftItem(Item item)
    {
        return ItemManager.Instance.AllRecipes.Any(a => a.Result == item);
    }

    public bool HasEnoughToCraft(Item item)
    {
        Recipe recipe = ItemManager.Instance.AllRecipes.Where(a => a.Result == item).FirstOrDefault();
        foreach (RecipesMaterial mats in recipe.Materials)
        {
            if (!inventorySystem.HasItemWithAmount(mats.Item, mats.Amount))
                return false;
        }
        return true;
    }

    public Recipe GetRecipeForItem(Item item)
    {
        return ItemManager.Instance.AllRecipes.Where(a => a.Result == item).FirstOrDefault();
    }


    public bool CraftItem(Item item)
    {
        if (!CanCraftItem(item))
            return false;

        Recipe recipe = GetRecipeForItem(item);

        foreach (RecipesMaterial mats in recipe.Materials)
        {
            inventorySystem.RemoveItem(mats.Item, mats.Amount);
        }
        inventorySystem.AddItem(recipe.Result, recipe.Amount);
        return true;
    }


    public void CraftXTimes(Recipe r, int x)
    {
        for(int i = 0; i < x; i++)
        {
            CraftItem(r.Result);
        }
    }

    internal List<KeyValuePair<Item, int>> GetMissingResources(Item item)
    {
        List<KeyValuePair<Item, int>> missingResources = new List<KeyValuePair<Item, int>>();

        Recipe rec = GetRecipeForItem(item);
        foreach (RecipesMaterial material in rec.Materials)
        {
            if (!inventorySystem.HasItemWithAmount(material.Item, material.Amount))
            {
                int missingAmount = Mathf.Max(material.Amount - inventorySystem.GetItemAmount(item), 0);
                missingResources.Add(new KeyValuePair<Item, int>(material.Item, missingAmount));
            }
        }
        return missingResources;

    }

    internal int GetMaximumCraftable(Recipe re)
    {
        List<int> maximumPerMaterial = new List<int>();

        foreach(RecipesMaterial rM in re.Materials)
        {
            int hasAmount = inventorySystem.GetItemAmount(rM.Item);
            int maximumpossible = hasAmount == 0 ? 0 : (hasAmount / rM.Amount);
            maximumPerMaterial.Add(maximumpossible);
        }

        return maximumPerMaterial.Min(); 
    }
}

