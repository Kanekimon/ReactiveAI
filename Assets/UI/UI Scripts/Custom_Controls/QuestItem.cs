using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class QuestItem : VisualElement
{
    public QuestItem(Quest q)
    {
        Label questTitle = new Label();
        questTitle.text = q.Description;

        Add(questTitle);
        questTitle.AddToClassList("quest-title");
        AddToClassList("quest-item");
    }

    public QuestItem() { }

    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<QuestItem, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
