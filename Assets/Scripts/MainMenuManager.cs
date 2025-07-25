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
    public Slider sfxVolumeSlider; // Thêm dòng này
    public TextMeshProUGUI sfxVolumeValueText; // Thêm dòng này
    [Header("SFX")]
    public AudioSource sfxSource;
    public AudioClip clickSound;

    void Start()

    {

        if (PlayerPrefs.HasKey("MasterVolume"))
        {
            float savedVolume = PlayerPrefs.GetFloat("MasterVolume");
            SetVolume(savedVolume);

            // Mẹo nhỏ: Cập nhật cả vị trí thanh trượt để khớp với âm lượng đã lưu
            if (volumeSlider != null)
            {
                volumeSlider.value = savedVolume;
            }
        }
        // Nếu đây là lần đầu tiên chơi, chưa có gì được lưu
        else
        {
            // Đặt âm lượng mặc định là 75%
            float defaultVolume = 0.75f;
            SetVolume(defaultVolume);

            // Mẹo nhỏ: Cập nhật cả vị trí thanh trượt để khớp với âm lượng mặc định
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
            // Đặt âm lượng SFX mặc định là 100%
            float defaultSFXVolume = 1f;
            SetSFXVolume(defaultSFXVolume);
            sfxVolumeSlider.value = defaultSFXVolume;
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

    public void SetSFXVolume(float volume)
    {
        if (sfxSource != null)
        {
            // Chỉ thay đổi âm lượng của loa SFX
            sfxSource.volume = volume;
        }

        if (sfxVolumeValueText != null)
        {
            sfxVolumeValueText.text = Mathf.RoundToInt(volume * 100) + "%";
        }

        // Lưu lại với một key khác để không bị đè lên âm lượng tổng
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }
}