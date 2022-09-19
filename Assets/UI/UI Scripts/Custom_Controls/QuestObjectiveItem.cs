using UnityEngine.Scripting;
using UnityEngine.UIElements;


public class QuestObjectiveItem : VisualElement
{

    public QuestObjectiveItem()
    {

    }

    public QuestObjectiveItem(QuestObjective qO)
    {
        Toggle t = new Toggle();
        t.label = "";
        t.text = "";
        t.value = qO.Completed;

        t.SetEnabled(false);

        Label desc = new Label();
        desc.text = qO.GetDescription();

        if (t.value)
        {
            desc.style.color = new StyleColor(new UnityEngine.Color32(90, 90, 90, 150));
        }

        Add(t);
        Add(desc);
        AddToClassList("horizontal-container");

    }


    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<QuestObjectiveItem, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}

