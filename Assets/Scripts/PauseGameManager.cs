using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PauseGameManager : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public GameObject inGameSettingsPanel;
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValueText;

    private bool isPaused = false;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetVolume(savedVolume);
            volumeSlider.value = savedVolume;
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused && !inGameSettingsPanel.activeSelf)
                ResumeGame();
            else if (!isPaused)
                PauseGame();
        }
    }

    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        inGameSettingsPanel.SetActive(false); // Ẩn settings nếu đang mở
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void PauseGame()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Main Menu");
    }

    public void OpenSettings()
    {
        pauseMenuUI.SetActive(false);
        inGameSettingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        inGameSettingsPanel.SetActive(false);
        pauseMenuUI.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        Debug.Log("Slider Volume: " + volume);
        AudioListener.volume = volume;

        if (volumeValueText != null)
            volumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";

        PlayerPrefs.SetFloat("MasterVolume", volume);
    }
}
