using UnityEngine;
using TMPro;

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
    public GameObject winPanel;
    public TextMeshProUGUI winText;
    public bool raceEnded { get; private set; } = false;
    public TextMeshProUGUI loseText;
    public GameObject player1ResultPanel;
    public GameObject player2ResultPanel;
    public TextMeshProUGUI player1ResultText;
    public TextMeshProUGUI player2ResultText;

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
        // Đọc game mode từ PlayerPrefs (mặc định VsHuman)
        int savedMode = PlayerPrefs.GetInt("GameMode", 0);
        GameMode modeToSet = (savedMode == 1) ? GameMode.VsAI : GameMode.VsHuman;

        SetGameMode(modeToSet);
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
    public void DeclareWinner(int winnerPlayerID)
    {
        if (raceEnded) return;

        raceEnded = true;

        player1ResultPanel.SetActive(false);
        player2ResultPanel.SetActive(false);

        int loserPlayerID = (winnerPlayerID == 1) ? 2 : 1;

        // Player 1 thắng
        if (winnerPlayerID == 1)
        {
            player1ResultPanel.SetActive(true);
            player1ResultText.text = "🎉 YOU WIN!";
            player2ResultPanel.SetActive(true);
            player2ResultText.text = "😞 YOU LOSE!";
        }
        else
        {
            player2ResultPanel.SetActive(true);
            player2ResultText.text = "🎉 YOU WIN!";
            player1ResultPanel.SetActive(true);
            player1ResultText.text = "😞 YOU LOSE!";
        }

        Time.timeScale = 0f;
    }
}