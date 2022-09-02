using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;

    public List<Item> AllItems = new List<Item>();
    public List<Recipe> AllRecipes = new List<Recipe>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private void Start()
    {

    }

    public Item GetItemByName(string name)
    {
        return AllItems.Where(a => a.Name.Equals(name)).FirstOrDefault();
    }

    public List<RecipesMaterial> GetMaterialsForItem(Item i)
    {
        foreach (Recipe recipe in AllRecipes)
        {
            if (recipe.Result == i)
            {
                return recipe.Materials;
            }
        }
        return null;
    }

}

