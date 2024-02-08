using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;
#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class PermissionRequest : SingletonMonobehaviour<PermissionRequest>
{
    private const string CAMERA_PERMISSION = Permission.Camera;
    private const string MICROPHONE_PERMISSION = Permission.Microphone;

    protected override void Awake()
    {
        base.Awake();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        RequestCameraPermission();
        RequesMicrophonePermission();
    }

    private static void RequestCameraPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(CAMERA_PERMISSION))
        {
            Permission.RequestUserPermission(CAMERA_PERMISSION);
        }
#elif UNITY_IOS
        if (!UnityEngine.Application.HasUserAuthorization(UnityEngine.UserAuthorization.WebCam ))
        {
            UnityEngine.Application.RequestUserAuthorization(UnityEngine.UserAuthorization.WebCam );
        }

        CLog.LogInfoOnceEditor(Application.HasUserAuthorization(UnityEngine.UserAuthorization.WebCam)
            ? "[PerformanceVoiceController-Start] Webcam found."
            : "[PerformanceVoiceController-Start] Webcam not found.");
#endif
    }

    private static void RequesMicrophonePermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(MICROPHONE_PERMISSION))
        {
            Permission.RequestUserPermission(MICROPHONE_PERMISSION);
        }
#elif UNITY_IOS
          // Request iOS Microphone permission
            Application.RequestUserAuthorization(UserAuthorization.Microphone);

            // Check iOS Microphone permission
            CLog.LogInfoOnceEditor(Application.HasUserAuthorization(UserAuthorization.Microphone)
                ? "[PerformanceVoiceController-Start] Microphone found."
                : "[PerformanceVoiceController-Start] Microphone not found.");
#endif
    }
}
