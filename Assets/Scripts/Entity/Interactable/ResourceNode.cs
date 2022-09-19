public class ResourceNode : Interactable
{
    PlayerSystem player;
    public InteractionType InteractionType;


    private void Start()
    {
        player = GameManager.Instance.Player;
        OnInteract = new UnityEngine.Events.UnityEvent();
        OnInteract.AddListener(() => player.InteractionSystem.Interact(InteractionType, this.gameObject));
    }
}

