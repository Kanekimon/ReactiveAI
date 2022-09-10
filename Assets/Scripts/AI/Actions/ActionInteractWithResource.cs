using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionInteractWithResource : ActionBase
{
    [SerializeField] int AmountToGather;
    private GameObject _target;
    private Item resourceToGather;
    private int gatherCount;
    private bool sameResource = false;

    private float timer;
    public float delay = 2f;

    protected override void Start()
    {
        
        preconditions.Add(new KeyValuePair<string, object>("isAtPosition", true));
        preconditions.Add(new KeyValuePair<string, object>("hasTool", true));
        effects.Add(new KeyValuePair<string, object>("interactWithResource", true));
        effects.Add(new KeyValuePair<string, object>("gatherResource", true));
        effects.Add(new KeyValuePair<string, object>("hasItem", true));
        base.Start();
    }

    public override bool CanRun()
    {
        Item requested = WorldState.GetValue<Item>("requestedItem");

        if (!WorldState.GetValue<bool>("hasTool"))
            return false;

        if(requested != null && !requested.HasRecipe && requested.IsResource)
        {
            return true;
        }

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
        sameResource = false;
    }

    public override void OnDeactived()
    {
        base.OnDeactived();
        Agent.GetComponent<Animator>().SetBool("isAttacking", false);

    }

    public override void OnTick()
    {
        
        GameObject tmp = WorldState.GetValue<GameObject>("target");

        if (_target == null)
        {
            if (!sameResource)
            {
                _target = tmp;
                sameResource = true;
            }
            else
            {
                this._needsReplanning = true;
                OnDeactived();
                return;
            }
        }



        if (timer >= delay || _target == null)
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

