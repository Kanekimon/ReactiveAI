using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionInteractWithResource : ActionBase
{
    [SerializeField] int AmountToGather;
    private GameObject _target;
    private Item resourceToGather;
    private int gatherCount;

    private float timer;
    public float delay = 2f;

    protected override void Start()
    {
        
        preconditions.Add(new KeyValuePair<string, object>("isAtPosition", true));
        effects.Add(new KeyValuePair<string, object>("interactWithResource", true));
        effects.Add(new KeyValuePair<string, object>("gatherResource", true));
        base.Start();
    }

    public override bool CanRun()
    {
        if (WorldState.GetValue<List<ResourceType>>("possibleResources") != null)
            return true;
        return false;
    }


    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);
        gatherCount = 0;
        AmountToGather = WorldState.GetValue<int>("gatherAmount");
        resourceToGather = WorldState.GetValue<Item>("requestedResource");
        Agent.GetComponent<Animator>().SetBool("isAttacking", true);
    }

    public override void OnDeactived()
    {
        base.OnDeactived();
        Agent.GetComponent<Animator>().SetBool("isAttacking", false);

    }

    public override void OnTick()
    {
        _target = WorldState.GetValue<GameObject>("target");


        if (timer >= delay)
        {

            timer = 0;
            if (_target != null && gatherCount < AmountToGather)
            {
                gatherCount += _target.GetComponent<ResourceTarget>().Interact(this.Agent).Sum(a => a.Amount);
            }
            else if (_target == null && gatherCount < AmountToGather)
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

