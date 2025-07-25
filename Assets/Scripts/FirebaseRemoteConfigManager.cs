using System;
using System.Collections.Generic;
using Firebase;
using Firebase.Extensions;
using Firebase.RemoteConfig;
using UnityEngine;

public class FirebaseRemoteConfigManager : MonoBehaviour
{
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
        string gameMode = FirebaseRemoteConfig.DefaultInstance.GetValue("game_mode").StringValue;

        Debug.Log("Game Mode từ Remote Config: " + gameMode);

        if (gameMode == "vs_ai")
        {
            Debug.Log("Đang chuyển sang chế độ chơi với AI");

            GameManager.Instance?.SetGameMode(GameManager.GameMode.VsAI);
        }
        else if (gameMode == "vs_human")
        {
            Debug.Log("Đang chuyển sang chế độ chơi với người");

            GameManager.Instance?.SetGameMode(GameManager.GameMode.VsHuman);
        }
        else
        {
            Debug.Log("Giá trị không xác định, fallback về vs_human");
            GameManager.Instance?.SetGameMode(GameManager.GameMode.VsHuman);
        }
    }
    catch (Exception ex)
    {
        Debug.LogError("Lỗi khi áp dụng Remote Config: " + ex.Message);
    }
}
}