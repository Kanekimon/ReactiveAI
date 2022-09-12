using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine.Scripting;
using UnityEngine.UIElements;

public class Notification : VisualElement
{

    public float Duration;

    public Notification()
    {
        Label message = new Label();
        message.text = "Test";

        Add(message);
        message.AddToClassList("notification-message");
        AddToClassList("notification");
    }

    public Notification(string n_message, float duration)
    {
        Duration = duration;

        Label message = new Label();
        message.text = n_message;

        Add(message);
        message.AddToClassList("notification-message");
        AddToClassList("notification");
    }



    #region UXML
    [Preserve]
    public new class UxmlFactory : UxmlFactory<Notification, UxmlTraits> { }
    [Preserve]
    public new class UxmlTraits : VisualElement.UxmlTraits { }
    #endregion
}
