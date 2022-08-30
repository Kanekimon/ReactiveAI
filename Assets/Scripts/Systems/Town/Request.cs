﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public enum RequestStatus
{
    Requested,
    InProgress,
    Ready,
    Finished
}

public class Request
{
    public Item RequestedItem;
    public int RequestedAmount;
    public RequestStatus Status;
    public GameObject Requester;
    public GameObject Giver;
    public Storage Storage;

    public Request(GameObject requester, Item item, int amount)
    {
        this.Requester = requester;
        RequestedItem = item;
        RequestedAmount = amount;
        this.Status = RequestStatus.Requested;
    }


    public void AssignToRequest(GameObject giver)
    {
        Giver = giver;
        this.Status = RequestStatus.InProgress;
    }

    public void ChangeAmount(int amount)
    {
        RequestedAmount += amount;
        this.Status = RequestStatus.Requested;
    }
    
    public void ChangeStatus(RequestStatus newStatus)
    {
        this.Status = newStatus;
    }

    public void SetStorage(Storage st)
    {
        Storage = st;
    }
}
