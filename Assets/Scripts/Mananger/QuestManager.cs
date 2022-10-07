using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    public static QuestManager Instance;

    public List<Quest> Quests = new List<Quest>();
    public List<Quest> Active = new List<Quest>();
    public List<Quest> Finished = new List<Quest>();

    public List<QuestObjective> AllObjectiveTypes = new List<QuestObjective>();

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
        if (Quests.Count > 0)
            StartQuest(Quests.First());
    }

    public void StartQuest(Quest activate)
    {
        if (Quests.Any(a => a == activate))
        {
            activate.Initialize();
            Active.Add(activate);
            Quests.Remove(activate);
        }
    }


    public void FinishQuest(Quest close)
    {
        if (Active.Any(a => a == close))
        {
            Finished.Add(close);
            Active.Remove(close);
        }
    }

    public bool HasQuestWithInteractionType(InteractionType inter)
    {
        foreach (Quest q in Active)
        {
            foreach (QuestObjective objective in q.Objectives)
            {
                if (objective is TeachObjective)
                {
                    if ((objective as TeachObjective).InteractionType == inter)
                        return true;
                }
            }
        }
        return false;
    }

    internal bool HasQuestWithItem(Item i)
    {
        foreach (Quest q in Active)
        {
            foreach (QuestObjective objective in q.Objectives)
            {
                if (objective is GatherObjective)
                {
                    if ((objective as GatherObjective).Item.Id == i.Id)
                        return true;
                }
            }
        }
        return false;
    }
}

