using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActionInteractWithResource : ActionBase
{
    [SerializeField] int AmountToGather;
    private GameObject _target;
    private Item resourceToGather;

    private float timer;
    public float delay = 2f;

    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("isAtResource", true));
        effects.Add(new KeyValuePair<string, object>("interactWithResource", true));
        effects.Add(new KeyValuePair<string, object>("hasItem", true));
        base.Start();
    }

    public override bool CanRun()
    {
        Item tmp = Agent.WorldState.GetValue("requestedItem") as Item;
        if(tmp != null && !tmp.IsResource)
        {
            return false;
        }
        return true;
    }


    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        
        AmountToGather = int.Parse(Agent.WorldState.GetValue("gatherAmount")?.ToString() ?? "1");
        resourceToGather = Agent.WorldState.GetValue("requestedItem") as Item;
    }


    public override void OnTick()
    {
        _target = Agent.WorldState.GetValue("wantedResource") as GameObject;
        if (timer >= delay)
        {
            timer = 0;
            if (_target != null && !Agent.InventorySystem.HasEnough(resourceToGather, AmountToGather))
            {
                _target.GetComponent<ResourceTarget>().Interact(this.Agent);
            }
            else if(_target == null && !Agent.InventorySystem.HasEnough(resourceToGather, AmountToGather))
            {
                this._needsReplanning = true;
            }
            else
            {
                OnDeactived();
            }
        }
        timer += Time.deltaTime;
    }
}

