using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class RecipeItem : VisualElement
{
    public Recipe Recipe;
    public string ItemName;


    public RecipeItem()
    {
        Image icon = new Image();
        Label recipe_name = new Label();

        recipe_name.text = "Test";


        Add(icon);
        Add(recipe_name);

        icon.AddToClassList("item-icon");
        recipe_name.AddToClassList("request-text");
        AddToClassList("recipe-item");
    }

    public RecipeItem(Recipe r)
    {
        Image icon = new Image();
        icon.sprite = r.Result.Icon;
        Label recipe_name = new Label();
        recipe_name.text = r.Result.Name;


        Add(icon);
        Add(recipe_name);

        icon.AddToClassList("item-icon");
        recipe_name.AddToClassList("request-text");
        AddToClassList("recipe-item");
    }




    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<RecipeItem, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

