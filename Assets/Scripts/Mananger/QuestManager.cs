using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        StartQuest(Quests.First());
    }

    public void StartQuest(Quest activate)
    {
        if(Quests.Any(a => a == activate))
        {
            activate.Initialize();
            Active.Add(activate);
            Quests.Remove(activate);
        }
    }

    public void FinishQuest(Quest close)
    {
        if(Active.Any(a => a == close))
        {
            Finished.Add(close);
            Active.Remove(close);
        }
    }




}

