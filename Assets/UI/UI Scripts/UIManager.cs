using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

struct View
{
    public VisualElement view;
    public UiView script;
}

public class UIManager : MonoBehaviour
{
    VisualElement baseRoot;
    VisualElement stamina_bar;
    VisualElement health_bar;
    VisualElement hunger_bar;
    VisualElement thirst_bar;

    PlayerConditionSystem player_condition;
    bool anyUiOpen = false;

    VisualElement currentlyOpen;
    Dictionary<string, View> views = new Dictionary<string, View>();

    // Start is called before the first frame update
    void Start()
    {
        player_condition = GameManager.Instance.Player.ConditionSystem;
        baseRoot = GetComponent<UIDocument>().rootVisualElement;

        stamina_bar = baseRoot.Q<VisualElement>("stamina_bar").Q<VisualElement>("bar_overlay");
        baseRoot.Q<VisualElement>("stamina_bar").Q<Label>("bar_label").text = "Stamina";

        health_bar = baseRoot.Q<VisualElement>("health_bar").Q<VisualElement>("bar_overlay");
        baseRoot.Q<VisualElement>("health_bar").Q<Label>("bar_label").text = "Health";

        hunger_bar = baseRoot.Q<VisualElement>("hunger_bar").Q<VisualElement>("bar_overlay");
        baseRoot.Q<VisualElement>("hunger_bar").Q<Label>("bar_label").text = "Hunger";

        thirst_bar = baseRoot.Q<VisualElement>("thirst_bar").Q<VisualElement>("bar_overlay");
        baseRoot.Q<VisualElement>("thirst_bar").Q<Label>("bar_label").text = "Thirst";

        UnityEngine.Cursor.visible = false;

        baseRoot.Query<VisualElement>().Where(a => a.name.ToLower().Contains("view")).ForEach(a => views.Add(a.name.Replace("_view", ""), new View() { view = a, script = (UiView) GetComponent(a.name)}));

        foreach(KeyValuePair<string, View> view in views)
        {
            Debug.Log($"View {view.Key}: {view.Value}");
        }

    }

    // Update is called once per frame
    void Update()
    {
        UpdateBars();

        if (Input.GetButtonDown("Inventory"))
        {
            ToggleWindow("Inventory");
        }


        UnityEngine.Cursor.visible = currentlyOpen != null;
    }

    public void ToggleWindow(string key)
    {
        VisualElement window = views[key].view;

        if (window.style.display == null || window.style.display == DisplayStyle.None)
        {
            if (currentlyOpen != null)
            {
                views.Where(a => a.Value.view.name.Equals(currentlyOpen.name)).FirstOrDefault().Value.script.Close();
                currentlyOpen.style.display = DisplayStyle.None;
            }

            currentlyOpen = window;
            window.style.display = DisplayStyle.Flex;
            views[key].script.Open();
        }
        else
        {
            currentlyOpen = null;
            window.style.display = DisplayStyle.None;
            views[key].script.Close();
        }
    }


    void UpdateBars()
    {
        SetBarColor(health_bar, player_condition.GetCondition("Health"));
        SetBarColor(stamina_bar, player_condition.GetCondition("Stamina"));
        SetBarColor(hunger_bar, player_condition.GetCondition("Hunger"));
        SetBarColor(thirst_bar, player_condition.GetCondition("Thirst"));
    }


    void SetBarColor(VisualElement bar, Condition c)
    {
        bar.style.width = Length.Percent(Mathf.FloorToInt((c.Value / c.MaximumValue) * 100));
        if (c.Value > c.MaximumValue/2)
        {
            bar.style.backgroundColor = new StyleColor(new Color32(144, 190, 109,255));
        }
        else if(c.Value <= c.MaximumValue / 2 && c.Value > c.MaximumValue / 4)
        {
            bar.style.backgroundColor = new StyleColor(new Color32(249, 199, 79, 255));

        }
        else
        {
            bar.style.backgroundColor = new StyleColor(new Color32(249, 65, 68, 255));
        }
    }
}
