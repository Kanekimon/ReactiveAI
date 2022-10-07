using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public PlayerSystem Player;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        this.Player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerSystem>();
    }




}
