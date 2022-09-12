using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class RecipeMaterial : VisualElement
{

    public RecipeMaterial()
    {
        Image icon = new Image();
        Label amount_needed = new Label();

        amount_needed.text = "2 x Wood";

        Add(icon);
        Add(amount_needed);


        icon.AddToClassList("recipe-material-icon");
        amount_needed.AddToClassList("recipe-material-amount-needed");
        AddToClassList("recipe-material");
    }

    public RecipeMaterial(Item item, int amount)
    {
        Image icon = new Image();
        icon.sprite = item.Icon;
        Label amount_needed = new Label();
        amount_needed.text = $"{amount}x {item.Name}";

        Add(icon);
        Add(amount_needed);


        icon.AddToClassList("recipe-material-icon");
        amount_needed.AddToClassList("recipe-material-amount-needed");
        AddToClassList("recipe-material");
    }



    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<RecipeMaterial, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
