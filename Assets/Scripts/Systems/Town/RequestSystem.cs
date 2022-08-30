using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RequestSystem : MonoBehaviour
{
    TownSystem townSystem;
    [SerializeField] List<Request> _open = new List<Request>();
    [SerializeField] List<Request> _inWork = new List<Request>();

    public List<Request> Open => _open;
    public List<Request> InWork => _inWork;

    private void Awake()
    {
        townSystem = this.transform.parent.GetComponent<TownSystem>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (_open.Count > 0)
        {
            for (int i = _open.Count - 1; i >= 0; i--)
            {
                if (townSystem.RequestResoruces(_open[i]))
                {
                    _inWork.Add(_open[i]);
                    _open.RemoveAt(i);
                }
            }
        }

        for (int i = _inWork.Count - 1; i >= 0; i--)
        {
            if (_inWork[i].Status == RequestStatus.Finished)
                _inWork.RemoveAt(i);
        }
    }

    public void RequestItem(Agent agent, Item item, int amount)
    {
        Request r = new Request(agent.gameObject, item, amount);
        _open.Add(r);
    }

    internal void FinishedRequest(Agent agent, Item itemDelivered, GameObject storage)
    {
        foreach (Request req in _inWork.Where(a => a.Giver == agent.gameObject))
        {
            if (req.RequestedItem == itemDelivered)
            {
                req.Storage = storage.GetComponent<Storage>();
                req.Status = RequestStatus.Finished;
                req.Requester.GetComponent<Agent>().PickUpResourceFromTarget(req);
            }
        }
    }
}
