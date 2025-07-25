using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance;

    private int player1Score = 0;
    private int player2Score = 0;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddScore(int playerID, int amount)
    {
        if (playerID == 1)
        {
            player1Score += amount;
            Debug.Log("Player 1 Score: " + player1Score);
        }
        else if (playerID == 2)
        {
            player2Score += amount;
            Debug.Log("Player 2 Score: " + player2Score);
        }
    }

    public int GetScore(int playerID)
    {
        return (playerID == 1) ? player1Score : player2Score;
    }
}
