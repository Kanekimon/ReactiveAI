using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class Townhall_view : UiView
{
    TownSystem Town;
    VisualElement villagerContainer;
    Label villagerName;
    Label villagerAge;
    Label villagerJob;
    Agent selected;

    VisualElement hire;
    VisualElement fire;


    DropdownField jobTypes;

    List<string> Jobs = new List<string>();

    private void Start()
    {
        villagerContainer = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager").Q<VisualElement>("villager-container");
        villagerName = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager-info").Q<VisualElement>("header").Q<Label>("villager-name");
        villagerAge = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager-info").Q<VisualElement>("body").Q<VisualElement>("villager-age").Q<Label>("villager-age-text");
        villagerJob = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager-info").Q<VisualElement>("body").Q<VisualElement>("villager-job").Q<Label>("villager-job-text");
        hire = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager-info").Q<VisualElement>("footer").Q<VisualElement>("villager-hire");
        hire.Q<Button>("villager-hire-button").clicked += HireHandler();
        jobTypes = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager-info").Q<VisualElement>("footer").Q<VisualElement>("villager-hire").Q<DropdownField>("villager-jobtypes");
        fire = Root.Q<VisualElement>("Townhall_view").Q<VisualElement>("Container").Q<VisualElement>("villager-info").Q<VisualElement>("footer").Q<VisualElement>("villager-fire");
        fire.Q<Button>("villager-fire-button").clicked += FireHandler();
        selected = null;

        Jobs = Enum.GetNames(typeof(JobType)).ToList();

        jobTypes.choices = Jobs;
    }

    public override void Open()
    {
        Town = GameManager.Instance.Player.CurrentTown;

        villagerContainer.Clear();
        Debug.Log("TestTown");
        Debug.Log($"Villager Count: {Town.Villager.Count}");

        foreach (Agent agent in Town.Villager)
        {
            VillagerItem vI = new VillagerItem(agent);
            vI.RegisterCallback<MouseDownEvent, Agent>(ShowVillagerInfo, agent);
            villagerContainer.Add(vI);
        }

    }

    private void ShowVillagerInfo(MouseDownEvent evt, Agent villager)
    {
        selected = villager;
        villagerName.text = selected.name;
        villagerAge.text = "10";
        villagerJob.text = villager.Job == null ? "None" : villager.Job.JobType.ToString();

        if(villager.Job == null)
        {
            fire.style.display = DisplayStyle.None;
            hire.style.display = DisplayStyle.Flex;
        }
        else
        {
            hire.style.display = DisplayStyle.None;
            fire.style.display = DisplayStyle.Flex;
        }
    }

    public Action HireHandler()
    {
        return () =>
        {
            JobType job = (JobType) Enum.Parse(typeof(JobType), jobTypes.choices[jobTypes.index]);
            Town.HireWorker(selected, Town.AvailableJobs.Where(a => a.JobType == job).FirstOrDefault());
        };
    }

    public Action FireHandler()
    {
        return () =>
        {
            JobType job = (JobType)Enum.Parse(typeof(JobType), jobTypes.choices[jobTypes.index]);
            Town.FireWorker(selected);
        };
    }

}

