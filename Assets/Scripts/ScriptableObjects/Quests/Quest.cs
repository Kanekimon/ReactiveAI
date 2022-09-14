using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName = "Quest")]
public class Quest : ScriptableObject
{
    protected int id;
    public string Name;
    public string Description;
    public bool IsActive;
    public List<Quest> NextQuests = new List<Quest>();

    public List<QuestObjective> Objectives = new List<QuestObjective>();

    public int Reward;

    public int GetId() => id;
    public bool CanBeCompleted { get; protected set; }
    public bool Completed { get; protected set; }
    
    public QuestCompletedEvent CompletedEvent;

    public void Initialize()
    {
        Completed = false;
        CanBeCompleted = false;
        CompletedEvent = new QuestCompletedEvent();
        CompletedEvent.AddListener(QuestManager.Instance.FinishQuest);

        foreach(var goal in Objectives)
        {
            goal.Initialize();
            goal.GoalCompleted.AddListener(delegate { CheckObjectives(); });
        }
    }

    private void CheckObjectives()
    {
        CanBeCompleted = Objectives.All(a => a.Completed);
    }

    public void CompleteQuest()
    {
        Completed = true;
        foreach (Quest q in NextQuests)
        {
            QuestManager.Instance.StartQuest(q);
        }

        GameManager.Instance.Player.AddReputation(Reward);
        CompletedEvent.Invoke(this);
        CompletedEvent.RemoveAllListeners();
    }



}




[SerializeField]
[CreateAssetMenu(menuName ="Quest/Objective")]
public abstract class QuestObjective : ScriptableObject
{
    protected int Id;
    protected string Description;
    [SerializeField] public int CurrentAmount { get; protected set; }
    public int RequiredAmount = 1;

    public bool Completed { get; protected set; }
    [HideInInspector] public UnityEvent GoalCompleted;

    public virtual string GetDescription()
    {
        return Description;
    }



    public virtual void Initialize()
    {
        Completed = false;
        GoalCompleted = new UnityEvent();
    }

    protected void Evaluate()
    {
        Debug.Log($"Amount: {CurrentAmount}/{RequiredAmount} ");
        if(CurrentAmount >= RequiredAmount)
        {
            Complete();
        }
    }

    private void Complete()
    {
        Completed = true;
        GoalCompleted.Invoke();
        GoalCompleted.RemoveAllListeners();
    }
}

public class QuestCompletedEvent : UnityEvent<Quest> { }


