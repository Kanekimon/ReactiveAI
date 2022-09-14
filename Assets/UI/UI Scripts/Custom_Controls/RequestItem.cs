using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class RequestItem : VisualElement
{
    public Image Icon;
    public Label Request_Text;
    public Request request;
    public Button Complete;
    public PlayerSystem Player;


    public RequestItem()
    {
        //Create a new Image element and add it to the root
        Icon = new Image();
        Add(Icon);

        Request_Text = new Label();
        Request_Text.text = "3x Wood";
        Add(Request_Text);

        Complete = new Button();
        Complete.text = "Complete";
        Complete.clicked += (() => { CompleteButton(); });
        Add(Complete);

        //Add USS style properties to the elements
        //Icon.AddToClassList("slotIcon");
        Request_Text.AddToClassList("request-text");
        Icon.AddToClassList("item-icon");
        Complete.AddToClassList("submit-button");
        AddToClassList("request-item");
    }


    public RequestItem(Request r)
    {
        Player = GameManager.Instance.Player;
        request = r;
        //Create a new Image element and add it to the root
        Icon = new Image();
        Icon.sprite = r.RequestedItem.Icon;
        Add(Icon);

        Request_Text = new Label();
        Request_Text.text = $"{r.RequestedAmount}x {r.RequestedItem.Name}";
        Add(Request_Text);

        if (r.Requester != GameManager.Instance.Player.gameObject)
        {
            Complete = new Button();
            Complete.text = "Complete";
            Complete.clicked += (() => { CompleteButton(); });
            Add(Complete);
        }
        else
        {
            if (r.Status == RequestStatus.Ready)
            {
                Complete = new Button();
                Complete.text = "Retrieve";
                Complete.clicked += (() => { RetrieveRequestedItem(r); });
                Add(Complete);
            }
            else
            {
                Complete = new Button();
                Complete.text = "Cancle";
                Complete.clicked += (() => { CancleRequest(r); });
                Add(Complete);
            }
        }

        //Add USS style properties to the elements
        //Icon.AddToClassList("slotIcon");
        Request_Text.AddToClassList("request-text");
        Icon.AddToClassList("item-icon");
        Complete.AddToClassList("submit-button");
        AddToClassList("request-item");
    }


    public void CompleteButton()
    {
        if(Player.InventorySystem.HasItemWithAmount(request.RequestedItem, request.RequestedAmount))
        {
            if (Player.InventorySystem.TransferItemToOtherInventory(request.Requester.GetComponent<Agent>().HomeTown.RequestBoard.GetComponent<InventorySystem>(), request.RequestedItem, request.RequestedAmount))
            { 
                request.Requester.GetComponent<Agent>().HomeTown.RequestBoard.FinishedRequest(request);
            }

        }
    }

    public void CancleRequest(Request r)
    {

    }

    public void RetrieveRequestedItem(Request r)
    {
        if(r.Storage.GetComponent<InventorySystem>().TransferItemToOtherInventory(Player.InventorySystem, r.RequestedItem, r.RequestedAmount))
        {
            r.Status = RequestStatus.Finished;
        }
        else
        {
            UIManager.Instance.CreateNotification("Not enough space in your inventory!", 1f);
        }
    }



    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<RequestItem, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

