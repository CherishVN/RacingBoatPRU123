using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

public class FirebaseRemoteConfigManager : MonoBehaviour
{
    void Awake()
{
    PlayerPrefs.DeleteKey("PlayerChoseGameMode"); 
    
}
    void Start()
    {
        Debug.Log("Bắt đầu khởi tạo Firebase...");
        // Khởi tạo Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.IsCompleted)
            {
                if (task.Result == DependencyStatus.Available)
                {
                    Debug.Log("Firebase dependencies sẵn sàng!");
                    InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + task.Result);
                }
            }
            else if (task.IsFaulted)
            {
                Debug.LogError("Firebase initialization failed: " + task.Exception);
            }
        });
    }

    void InitializeFirebase()
    {
        Debug.Log("Đang thiết lập Remote Config...");
        // Thiết lập giá trị mặc định cho Remote Config
        Dictionary<string, object> defaults = new Dictionary<string, object>();
        defaults.Add("game_mode", "vs_human");  // Giá trị mặc định là chế độ vs người

        // Áp dụng giá trị mặc định
        FirebaseRemoteConfig.DefaultInstance.SetDefaultsAsync(defaults)
            .ContinueWithOnMainThread(task => 
            {
                if (task.IsFaulted)
                {
                    Debug.LogError("Lỗi khi thiết lập giá trị mặc định: " + task.Exception);
                    return;
                }
                Debug.Log("Đã thiết lập giá trị mặc định thành công!");
                FetchRemoteConfig();
            });
    }

    void FetchRemoteConfig()
    {
        // Lấy giá trị từ Remote Config
        FirebaseRemoteConfig.DefaultInstance.FetchAsync(TimeSpan.Zero).ContinueWithOnMainThread(fetchTask => 
        {
            if (fetchTask.IsFaulted)
            {
                Debug.LogError("Error fetching remote config: " + fetchTask.Exception);
                return;
            }

            if (fetchTask.IsCompleted)
            {
                // Kích hoạt các giá trị đã fetch
                FirebaseRemoteConfig.DefaultInstance.ActivateAsync()
                    .ContinueWithOnMainThread(activateTask =>
                    {
                        ApplyRemoteConfig();
                    });
            }
        });
    }

    void ApplyRemoteConfig()
    {
        try
    {
        // Nếu người chơi đã chọn chế độ thủ công từ menu, không ghi đè
        if (PlayerPrefs.GetInt("PlayerChoseGameMode", 0) == 1)
        {
            Debug.Log("Người chơi đã chọn chế độ chơi thủ công. Bỏ qua Remote Config.");
            return;
        }

        ConfigValue gameModeValue = FirebaseRemoteConfig.DefaultInstance.GetValue("game_mode");
        string gameMode = gameModeValue.StringValue;

        Debug.Log("Game Mode từ Remote Config: " + gameMode);

        if (gameMode == "vs_ai")
        {
            PlayerPrefs.SetInt("GameMode", 1);
            Debug.Log("Đã thiết lập chế độ chơi với AI từ Remote Config");
        }
        else if (gameMode == "vs_human")
        {
            PlayerPrefs.SetInt("GameMode", 0);
            Debug.Log("Đã thiết lập chế độ chơi với người từ Remote Config");
        }
        else
        {
            Debug.LogWarning("Giá trị game_mode không hợp lệ từ Remote Config. Dùng mặc định vs_ai");
            PlayerPrefs.SetInt("GameMode", 1);
        }

        PlayerPrefs.Save();
    }
    catch (Exception ex)
    {
        Debug.LogError("Lỗi khi áp dụng Remote Config: " + ex.Message);
    }
    }
}