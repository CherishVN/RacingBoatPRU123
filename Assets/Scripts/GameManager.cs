using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameMode
    {
        VsHuman,
        VsAI
    }

    public GameMode currentMode = GameMode.VsHuman;
    public Transform player1Boat;
    public Transform player2Boat;
    private BoatAIController aiController;
    private BoatMovementArrows player2Movement;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        // Lấy reference đến các component
        if (player2Boat != null)
        {
            aiController = player2Boat.GetComponent<BoatAIController>();
            player2Movement = player2Boat.GetComponent<BoatMovementArrows>();
        }

        SetGameMode(currentMode);
    }

    public void SetGameMode(GameMode mode)
    {
        currentMode = mode;
        if (aiController != null && player2Movement != null)
        {
            switch (mode)
            {
                case GameMode.VsHuman:
                    aiController.EnableAI(false);
                    player2Movement.isAIControlled = false;
                    player2Movement.enabled = true;
                    break;

                case GameMode.VsAI:
                    aiController.EnableAI(true);
                     player2Movement.isAIControlled = true;
                    player2Movement.enabled = true;
                    break;
            }
        }
    }
}
