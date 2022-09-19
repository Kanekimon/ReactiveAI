using UnityEngine;

public class TeachObjective : QuestObjective
{
    public InteractionType InteractionType;
    public int Amount;

    public override void Initialize()
    {
        base.Initialize();
        EventManager.Instance.AddListener<TeachGameEvent>(OnTeach);
    }

    private void OnTeach(TeachGameEvent eventInfo)
    {
        Debug.Log($"Action teaching: {InteractionType.ToString()}");
        if (eventInfo.Action == InteractionType && GameManager.Instance.Player.GetComponent<PlayerInteractionSystem>().HasFollower)
        {
            CurrentAmount++;
            Evaluate();
        }
    }

}
