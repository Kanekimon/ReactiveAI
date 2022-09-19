public class AgentInteractable : Interactable
{
    PlayerSystem player;
    public InteractionType InteractionType = InteractionType.communicate;


    private void Start()
    {
        player = GameManager.Instance.Player;
        OnInteract = new UnityEngine.Events.UnityEvent();
        OnInteract.AddListener(() => player.InteractionSystem.Interact(InteractionType, this.gameObject));
    }
}

