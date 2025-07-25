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
    public Slider sfxVolumeSlider;
    public TextMeshProUGUI sfxVolumeValueText;

    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip clickSound;

    void Start()
    {
        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetVolume(savedVolume);

            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume;
            }
        }
        else
        {
            float defaultVolume = 0.75f;
            SetVolume(defaultVolume);

            if (volumeSlider != null)
            {
                volumeSlider.value = defaultVolume;
            }
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float savedSFXVolume = PlayerPrefs.GetFloat("SFXVolume");
            SetSFXVolume(savedSFXVolume);
            sfxVolumeSlider.value = savedSFXVolume;
        }
        else
        {
            float defaultSFXVolume = 1f;
            SetSFXVolume(defaultSFXVolume);
            sfxVolumeSlider.value = defaultSFXVolume;
        }
    }

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            sfxSource.volume = volume;
        }

        if (sfxVolumeValueText != null)
        {
            sfxVolumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";
        }

        PlayerPrefs.SetFloat("SFXVolume", volume);
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
        SceneManager.LoadScene("OutdoorsScene");
    }

    public void StartAIMode()
    {
        PlayerPrefs.SetInt("GameMode", 1);
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
    public void PlayClickSound()
    {
        if (sfxSource != null && clickSound != null)
        {
            sfxSource.PlayOneShot(clickSound);
        }
    }
}