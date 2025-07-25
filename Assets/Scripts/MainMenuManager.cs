using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        // Load scene chơi game
        SceneManager.LoadScene("OutdoorsScene"); // Thay tên scene của bạn vào đây
    }

    public void QuitGame()
    {
        Debug.Log("Thoát game");
        Application.Quit();
    }
}
