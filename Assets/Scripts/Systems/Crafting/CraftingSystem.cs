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
        return Learned.Any(a => a.Result == item);
    }

    public bool HasEnoughToCraft(Item item)
    {
        Recipe recipe = Learned.Where(a => a.Result == item).FirstOrDefault();
        foreach (RecipesMaterial mats in recipe.Materials)
        {
            if (!inventorySystem.HasEnough(mats.Item, mats.Amount))
                return false;
        }
        return true;
    }

    public Recipe GetRecipeForItem(Item item)
    {
        return Learned.Where(a => a.Result == item).FirstOrDefault();
    }


    public void CraftItem(Item item)
    {
        if (!CanCraftItem(item))
            return;

        Recipe recipe = GetRecipeForItem(item);

        foreach (RecipesMaterial mats in recipe.Materials)
        {
            inventorySystem.RemoveItem(mats.Item, mats.Amount);
        }
        inventorySystem.AddItem(recipe.Result, recipe.Amount);

    }

    internal List<KeyValuePair<Item, int>> GetMissingResources(Item item)
    {
        List<KeyValuePair<Item, int>> missingResources = new List<KeyValuePair<Item, int>>();

        Recipe rec = GetRecipeForItem(item);
        foreach (RecipesMaterial material in rec.Materials)
        {
            if (!inventorySystem.HasEnough(material.Item, material.Amount))
            {
                int missingAmount = Mathf.Max(material.Amount - inventorySystem.GetItemAmount(item), 0);
                missingResources.Add(new KeyValuePair<Item, int>(material.Item, missingAmount));
            }
        }
        return missingResources;

    }
}

