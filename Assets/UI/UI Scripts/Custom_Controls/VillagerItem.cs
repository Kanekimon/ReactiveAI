using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class VillagerItem : VisualElement
{
    Agent villager;

    public VillagerItem()
    {

    }

    public VillagerItem(Agent pvillager)
    {
        this.villager = pvillager;
        Label name = new Label();
        name.text = villager.name;

        Add(name);
        name.AddToClassList("quest-title");
        AddToClassList("quest-item");
    }



    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<VillagerItem, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

