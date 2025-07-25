using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public class MainMenuManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject mainMenuPanel;
    public GameObject settingsPanel;
    public GameObject gameModePanel;

    [Header("UI Elements")]
    public Slider volumeSlider;
    public TextMeshProUGUI volumeValueText;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetVolume(savedVolume);
        }
        else
        {
            SetVolume(1f);
        }
    }

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume;

        if (volumeValueText != null)
        {
            volumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";
        }

        PlayerPrefs.SetFloat("MasterVolume", volume);
    }

    public void OpenGameModePanel()
    {
        mainMenuPanel.SetActive(false);
        gameModePanel.SetActive(true);
    }

    public void OpenSettings()
    {
        mainMenuPanel.SetActive(false);
        settingsPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Đã nhấn nút Thoát!");
        Application.Quit();
    }

    public void Start1v1Mode()
    {
         PlayerPrefs.SetInt("GameMode", 0);
         PlayerPrefs.SetInt("PlayerChoseGameMode", 1);
        SceneManager.LoadScene("OutdoorsScene");
    }

    public void StartAIMode()
    {
        PlayerPrefs.SetInt("GameMode", 1);
        PlayerPrefs.SetInt("PlayerChoseGameMode", 1);
        SceneManager.LoadScene("OutdoorsScene");
    }

    public void CloseGameModePanel()
    {
        gameModePanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        mainMenuPanel.SetActive(true);
    }
    

}