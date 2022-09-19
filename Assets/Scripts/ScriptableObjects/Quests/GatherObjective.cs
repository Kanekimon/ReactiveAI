using UnityEngine;

[SerializeField]
[CreateAssetMenu(menuName = "Quest/GatherObjective")]
public class GatherObjective : QuestObjective
{
    public Item Item;


    public override string GetDescription()
    {
        return $"Gather {RequiredAmount}x {Item.Name}";
    }

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<GatherGameEvent>(OnGather);
    }

    private void OnGather(GatherGameEvent eventInfo)
    {
        Debug.Log($"Gathered {eventInfo.Amount}x {eventInfo.Item}");
        if (eventInfo.Item == Item)
        {
            CurrentAmount += eventInfo.Amount;
            Evaluate();
        }
    }
}

