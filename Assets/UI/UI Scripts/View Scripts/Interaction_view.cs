using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.UIElements;

public class Interaction_view : UiView
{
    
    struct Inventories
    {
        public InventorySystem left;
        public InventorySystem right;
    }

    PlayerInteractionSystem interSystem;
    Inventories invs; 

    private void Start()
    {
        interSystem = GameManager.Instance.Player.GetComponent<PlayerInteractionSystem>();
    }


    public override void Open()
    {
        base.Open();
    }

    public override void Open(InventorySystem left, InventorySystem right)
    {
        invs = new Inventories();
        invs.left = left;
        invs.right = right;
        Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Inventory-Action").RegisterCallback<MouseDownEvent, Inventories>(OpenBothInventories, invs);

        if (interSystem.HasFollower)
        {
            Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Follow-Action").Q<Label>("Follow-Action-Label").text = "Stop Following";
        }
        else
        {
            Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Follow-Action").Q<Label>("Follow-Action-Label").text = "Following Player";
        }

        if (interSystem.HasFollower)
        {
            Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Follow-Action").RegisterCallback<MouseDownEvent, bool>(FollowPlayer, false);
        }
        else
        {
            Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Follow-Action").RegisterCallback<MouseDownEvent, bool>(FollowPlayer, true);
        }



    }

    public override void Close()
    {
        Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Follow-Action").UnregisterCallback<MouseDownEvent, bool>(FollowPlayer);
        Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Follow-Action").UnregisterCallback<MouseDownEvent, bool>(FollowPlayer);
        Root.Q<VisualElement>("Interaction_view").Q<VisualElement>("Menu").Q<VisualElement>("Inventory-Action").UnregisterCallback<MouseDownEvent, Inventories>(OpenBothInventories);
    }


    void OpenBothInventories(MouseDownEvent evt, Inventories invs)
    {
        UIManager.Instance.ToggleWindow("TwoInventory", invs.left, invs.right);
    }

    void FollowPlayer(MouseDownEvent evt, bool shouldFollow)
    {
        GameManager.Instance.Player.GetComponent<PlayerInteractionSystem>().SetFollowingAgent(shouldFollow);
        UIManager.Instance.CloseOpenView();
        
    }

}

