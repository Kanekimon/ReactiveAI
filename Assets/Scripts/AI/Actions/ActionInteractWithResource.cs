using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class ActionInteractWithResource : ActionBase
{
    [SerializeField] int AmountToGather;
    protected override void Start()
    {
        preconditions.Add(new KeyValuePair<string, object>("isAtResource", true));
        effects.Add(new KeyValuePair<string, object>("interactWithResource", true));
        base.Start();
    }


    public override void OnActivated(BaseGoal linked)
    {
        base.OnActivated(linked);

        AmountToGather = int.Parse(Agent.WorldState.GetValue("gatherAmount").ToString());
    }


    public override void OnTick()
    {
        if (AmountToGather > 0)
        {
            Agent.InventorySystem.AddItem(new Item() { Id = 1, ItemTypes = new List<ItemType>() { ItemType.Armor }, Name = "stone" }, 1);
            AmountToGather--;
        }
        else
        {
            _hasFinished = true;
            OnDeactived();
        }
    }
}

