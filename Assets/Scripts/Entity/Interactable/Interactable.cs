using UnityEngine;
using UnityEngine.Events;

public class Interactable : MonoBehaviour
{

    public bool Visible;
    [SerializeField] protected UnityEvent OnInteract;
    [SerializeField] protected ToolType ToolNeeded;


    private void Awake()
    {
    }
    protected virtual void Update()
    {
        if (Visible)
        {


            if (Input.GetKeyDown(KeyCode.E))
            {
                if (ToolNeeded != ToolType.none)
                {
                    ToolType equippedType = GameManager.Instance.Player.GetEquippedToolType();
                    if (equippedType != ToolNeeded)
                    {
                        UIManager.Instance.CreateNotification("You don't have the correct tool equipped!", 2f);
                        return;
                    }
                }

                OnInteract.Invoke();
            }
        }
    }
}

