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
    Label quest_view_description;
    VisualElement quest_view_reward;
    VisualElement quest_view_objective_container;
    Quest currentlySelected;

    private void Start()
    {
        active_container = Root.Q<VisualElement>("Quest_view").Q<VisualElement>("Container").Q<VisualElement>("Quest").Q<VisualElement>("quests").Q<VisualElement>("active-quests").Q<VisualElement>("active-quests-container");
        finished_container = Root.Q<VisualElement>("Quest_view").Q<VisualElement>("Container").Q<VisualElement>("Quest").Q<VisualElement>("quests").Q<VisualElement>("finished-quests").Q<VisualElement>("finished-quests-container");
        quest_view_container = Root.Q<VisualElement>("Quest_view").Q<VisualElement>("Container").Q<VisualElement>("Quest").Q<VisualElement>("Container").Q<VisualElement>("quest-info");
        quest_view_description = quest_view_container.Q<VisualElement>("body").Q<VisualElement>("quest-description").Q<Label>("description-text");
        quest_view_objective_container = quest_view_container.Q<VisualElement>("body").Q<VisualElement>("quest-objectives").Q<VisualElement>("quest-objectives-container");
    }

    public override void Open()
    {
        active_container.Clear();
        finished_container.Clear();

        if (currentlySelected != null)
        {
            ChangeQuestView(null, currentlySelected);
        }
        else
        {
            ShowCleanInfo();
        }

        foreach (Quest q in QuestManager.Instance.Active)
        {
            QuestItem qI = new QuestItem(q);
            qI.RegisterCallback<MouseDownEvent, Quest>(ChangeQuestView, q);
            active_container.Add(qI);
        }
        foreach (Quest q in QuestManager.Instance.Finished)
        {
            QuestItem qI = new QuestItem(q);
            qI.RegisterCallback<MouseDownEvent, Quest>(ChangeQuestView, q);
            finished_container.Add(qI);
        }
    }

    private void ShowCleanInfo()
    {
        quest_view_container.Q<VisualElement>("header").Q<Label>("quest-name").text = "Quest Journal";
        quest_view_description.text = "You can view and complete quests in here";
        quest_view_objective_container.Clear();
        SetVisibilityOfDetails(false);
    }

    private void ChangeQuestView(MouseDownEvent evt, Quest quest)
    {
        currentlySelected = quest;
        quest_view_container.Q<VisualElement>("header").Q<Label>("quest-name").text = $"{quest.Name}";
        SetVisibilityOfDetails(true);

        quest_view_description.text = quest.Description;
        quest_view_objective_container.Clear();

        foreach (QuestObjective qO in quest.Objectives)
        {
            quest_view_objective_container.Add(new QuestObjectiveItem(qO));
        }
        quest_view_container.Q<VisualElement>("footer").Q<Button>("complete-button").SetEnabled(quest.CanBeCompleted && !quest.Completed);


        quest_view_container.Q<VisualElement>("footer").Q<Button>("complete-button").clicked -= CompleteQuestHandler();
        currentlySelected = quest;
        quest_view_container.Q<VisualElement>("footer").Q<Button>("complete-button").clicked += CompleteQuestHandler();
    }

    public Action CompleteQuestHandler()
    {
        return () =>
        {
            currentlySelected.CompleteQuest();
            Refresh();
        };
    }

    public override void Refresh()
    {
        Open();
    }

    public void SetVisibilityOfDetails(bool visible)
    {
        quest_view_container.Q<VisualElement>("footer").Q<Button>("complete-button").visible = visible;
        quest_view_container.Q<VisualElement>("body").Q<VisualElement>("quest-reward").visible = visible;
        quest_view_container.Q<VisualElement>("body").Q<VisualElement>("quest-objectives").visible = visible;
    }

}

