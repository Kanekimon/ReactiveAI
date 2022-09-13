using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

[SerializeField]
[CreateAssetMenu(menuName = "Quest/GatherObjective")]
public class GatherObjective : QuestObjective
{
    public Item Item;


    public override string GetDescription()
    {
        return $"Gather {Item.Name}x {RequiredAmount}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<GatherGameEvent>(OnGather);
    }

    private void OnGather(GatherGameEvent eventInfo)
    {
        Debug.Log($"Gathered {eventInfo.Amount}x {eventInfo.Item}");
        if(eventInfo.Item == Item)
        {
            CurrentAmount += eventInfo.Amount;
            Evaluate();
        }
    }
}

