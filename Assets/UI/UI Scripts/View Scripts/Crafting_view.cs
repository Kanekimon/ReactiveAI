using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Crafting_view : UiView
{

    VisualElement recipes_container;
    VisualElement recipes_view_container;
    PlayerSystem player;
    CraftingSystem playerCrafting;

    Recipe currentlySelected;

    private void Start()
    {
        player = GameManager.Instance.Player;
        playerCrafting = player.CraftingSystem;
        recipes_container = Root.Q<VisualElement>("Crafting_view").Q<VisualElement>("Container").Q<VisualElement>("Crafting").Q<VisualElement>("Container").Q<VisualElement>("recipes");
        recipes_view_container = Root.Q<VisualElement>("Crafting_view").Q<VisualElement>("Container").Q<VisualElement>("Crafting").Q<VisualElement>("Container").Q<VisualElement>("recipe-info");
    }


    public override void Open()
    {
        foreach (var recipe in ItemManager.Instance.AllRecipes)
        {
            RecipeItem re = new RecipeItem(recipe);


            re.RegisterCallback<MouseDownEvent, Recipe>(ChangeRecipeView, recipe);

            recipes_container.Add(re);
        }
    }


    public override void Close()
    {
        recipes_container.Clear();
        base.Close();
    }


    public void ChangeRecipeView(MouseDownEvent e, Recipe re)
    {
        recipes_view_container.Q<VisualElement>("header").Q<Label>("recipe_name").text = $"{re.Result.Name}  (Result amount: ({re.Amount}))";
        recipes_view_container.Q<VisualElement>("body").Clear();
        foreach (RecipesMaterial rM in re.Materials)
        {
            recipes_view_container.Q<VisualElement>("body").Add(new RecipeMaterial(rM.Item, rM.Amount));
        }

        recipes_view_container.Q<VisualElement>("footer").Q<Label>("max-possible").text = $"Maximum possbile: ({playerCrafting.GetMaximumCraftable(re)})";


        Action value = () =>
                {
                    string numbterTimes = recipes_view_container.Q<VisualElement>("footer").Q<TextField>("craft-amount").text;
                    int amount = int.Parse(numbterTimes);
                    if (amount > playerCrafting.GetMaximumCraftable(currentlySelected))
                        UIManager.Instance.CreateNotification("Not enough materials", 2f);
                    else
                        playerCrafting.CraftXTimes(currentlySelected, amount);
                };

        recipes_view_container.Q<VisualElement>("footer").Q<Button>("craft-button").clicked -= CraftItemHandler();
        currentlySelected = re;
        recipes_view_container.Q<VisualElement>("footer").Q<Button>("craft-button").clicked += CraftItemHandler();

    }

    public Action CraftItemHandler()
    {
        return () =>
        {
            string numbterTimes = recipes_view_container.Q<VisualElement>("footer").Q<TextField>("craft-amount").text;
            int amount = int.Parse(numbterTimes);
            if (amount > playerCrafting.GetMaximumCraftable(currentlySelected))
                UIManager.Instance.CreateNotification("Not enough materials", 2f);
            else
                playerCrafting.CraftXTimes(currentlySelected, amount);
        };
    }


}

