using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class InventorySlot : VisualElement
{
    public Image Icon;
    public Label Amount;
    public Item Item;


    public InventorySlot()
    {
        //Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);
        //Add USS style properties to the elements
        Icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
    }

    public InventorySlot(Item item)
    {
        Item = item;
        //Create a new Image element and add it to the root
        Icon = new Image();
        this.Icon.sprite = item.Icon;
        Add(Icon);
        //Add USS style properties to the elements
        Icon.AddToClassList("slotIcon");
        AddToClassList("slotContainer");
    }

    public void AddAmount(int amount)
    {
        if (Amount == null)
        {
            Amount = new Label();
            Amount.AddToClassList("item_amount");
            Add(Amount);
        }

        Amount.text = amount.ToString();

    }


    public void HoldItem(Item item)
    {
        Icon.image = item.Icon.texture;
    }
    public void DropItem()
    {
        Icon.image = null;
        if (Amount != null)
            Remove(Amount);
        Amount = null;
    }




    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<InventorySlot, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
