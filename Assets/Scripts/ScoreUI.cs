using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI player1ScoreText;
    public TextMeshProUGUI player2ScoreText;

    void Update()
    {
        if (ScoreManager.Instance != null)
        {
            int p1 = ScoreManager.Instance.GetScore(1);
            int p2 = ScoreManager.Instance.GetScore(2);

            player1ScoreText.text = "Player 1: " + p1;
            player2ScoreText.text = "Player 2: " + p2;
        }
    }
}
