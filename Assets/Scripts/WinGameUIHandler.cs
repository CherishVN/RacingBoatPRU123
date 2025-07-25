using UnityEngine;
using UnityEngine.SceneManagement;

public class WinGameUIHandler : MonoBehaviour
{
    public void RestartLevel()
    {
        if (GameManager.Instance != null)
    {
        PlayerPrefs.SetInt("GameMode", GameManager.Instance.currentMode == GameManager.GameMode.VsAI ? 1 : 0);
    }

    Time.timeScale = 1f;
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }


}
