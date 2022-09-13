using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Quest_view : UiView
{

    VisualElement active_container;
    VisualElement finished_container;
    VisualElement quest_view_container;

    Quest currentlySelected;

    private void Start()
    {
        active_container = Root.Q<VisualElement>("Quest_view").Q<VisualElement>("Container").Q<VisualElement>("Quest").Q<VisualElement>("quests").Q<VisualElement>("active-quests").Q<VisualElement>("active-quests-container");
        finished_container = Root.Q<VisualElement>("Quest_view").Q<VisualElement>("Container").Q<VisualElement>("Quest").Q<VisualElement>("quests").Q<VisualElement>("finished-quests").Q<VisualElement>("finished-quests-container");
        quest_view_container = Root.Q<VisualElement>("Quest_view").Q<VisualElement>("Container").Q<VisualElement>("Quest").Q<VisualElement>("Container").Q<VisualElement>("quest-info");
    }

    public override void Open()
    {
        active_container.Clear();
        finished_container.Clear();

        foreach(Quest q in QuestManager.Instance.Active)
        {
            QuestItem qI = new QuestItem(q);
            qI.RegisterCallback<MouseDownEvent, Quest>(ChangeQuestView, q);
            active_container.Add(qI);
        }
        foreach(Quest q in QuestManager.Instance.Finished)
        {
            QuestItem qI = new QuestItem(q);
            qI.RegisterCallback<MouseDownEvent, Quest>(ChangeQuestView, q);
            finished_container.Add(qI);
        }
    }

    private void ChangeQuestView(MouseDownEvent evt, Quest quest)
    {
        quest_view_container.Q<VisualElement>("header").Q<Label>("quest-name").text = $"{quest.Name}";
        quest_view_container.Q<VisualElement>("body").Clear();
        Label quest_description = new Label();
        quest_description.text = quest.Description;
        quest_view_container.Q<VisualElement>("body").Add(quest_description);

        //quest_view_container.Q<VisualElement>("footer").Q<Button>("complete-button").clicked -= CraftItemHandler();
        //currentlySelected = quest;
        //quest_view_container.Q<VisualElement>("footer").Q<Button>("complete-button").clicked += CraftItemHandler();
    }

}

