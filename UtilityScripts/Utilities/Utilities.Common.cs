using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System;

public partial class MyUtilities {

    #region ----- Variables -----
    
    private static Dictionary<string, Shader> _shaderLookup = new Dictionary<string, Shader>();
    private static List<RaycastResult> _raycastList = new List<RaycastResult>();
    private static float _cachedScreenRatio = -1f;
    
    #endregion

    #region ----- Methods -----

    public static string GenerateUniqueId()
    {
        return Guid.NewGuid().ToString();
    }
    
    public static float DeviceDiagonalSizeInInches()
    {
        float screenWidth = Screen.width / Screen.dpi;
        float screenHeight = Screen.height / Screen.dpi;
        float diagonalInches = Mathf.Sqrt(Mathf.Pow(screenWidth, 2) + Mathf.Pow(screenHeight, 2));

        return diagonalInches;
    }

    public static float GetScreenRatio()
    {
        if (_cachedScreenRatio < 0f)
            _cachedScreenRatio = Screen.width / (float)Screen.height;

        return _cachedScreenRatio;
    }   

    public static Shader GetShader(string shaderName)
    {
        Shader tempShader = null;
        if (_shaderLookup.ContainsKey(shaderName))
            return _shaderLookup[shaderName];
      
        tempShader = Shader.Find(shaderName);
        if (tempShader != null)
            _shaderLookup.Add(shaderName, tempShader);
     
        return tempShader;
    }

    public static bool IsAnyPointerOverUI()
    {
        EventSystem es = EventSystem.current;
        if (es == null)
            return false;

        bool result = es.IsPointerOverGameObject();
        foreach (Touch touch in Input.touches)
            result |= es.IsPointerOverGameObject(touch.fingerId);

        return result;
    }
    
    public static bool IsAnyPointerOverUI_Ver2(params int[] ignoreRaycastArr)
    {
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        Vector2 pos = Vector2.zero;
        pos.x = Input.mousePosition.x;
        pos.y = Input.mousePosition.y;
        eventData.position = pos;

        _raycastList.Clear();
        EventSystem.current.RaycastAll(eventData, _raycastList);

        if (ignoreRaycastArr != null && ignoreRaycastArr.Length > 0)
        {
            for (int i = _raycastList.Count - 1; i >= 0; i--)
            {
                RaycastResult result = _raycastList[i];
                GameObject resultGo = result.gameObject;

                if (!resultGo)
                    continue;

                if (Array.BinarySearch(ignoreRaycastArr, resultGo.layer) >= 0)
                    _raycastList.Remove(result);
            }
        }
      
        return _raycastList.Count > 0;
    }

    public static string Md5Sum(string strToEncrypt)
    {
        System.Text.UTF8Encoding ue = new System.Text.UTF8Encoding();
        byte[] bytes = ue.GetBytes(strToEncrypt);

        // encrypt bytes
        System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
        byte[] hashBytes = md5.ComputeHash(bytes);

        // Convert the encrypted bytes back to a string (base 16)
        string hashString = "";
        for (int i = 0; i < hashBytes.Length; i++)
            hashString += System.Convert.ToString(hashBytes[i], 16).PadLeft(2, '0');

        return hashString.PadLeft(32, '0');
    }

    public static Vector4 GetWorldCameraBoundary(Camera camera, Vector3 cameraPosition)
    {
        float orthographicSize = camera ? camera.orthographicSize : 0f;
        float screenRatio = GetScreenRatio();
        
        Vector2 halfCameraSize = Vector2.zero;
        halfCameraSize.x = orthographicSize * screenRatio;
        halfCameraSize.y = orthographicSize;
        
        var cameraLocalBoundary = Vector4.zero;
        cameraLocalBoundary.x = -halfCameraSize.x;
        cameraLocalBoundary.y = halfCameraSize.x;
        cameraLocalBoundary.z = -halfCameraSize.y;
        cameraLocalBoundary.w = halfCameraSize.y;
        
        var worldCameraBoundary = Vector4.one;
        worldCameraBoundary.x = cameraPosition.x + cameraLocalBoundary.x;
        worldCameraBoundary.y = cameraPosition.x + cameraLocalBoundary.y;
        worldCameraBoundary.z = cameraPosition.y + cameraLocalBoundary.z;
        worldCameraBoundary.w = cameraPosition.y + cameraLocalBoundary.w;

        return worldCameraBoundary;
    }
    
    public static Vector2 GetCenterPosition(RectTransform rt)
    {
        Vector2 rect = rt.rect.size;
        Vector2 pivot = rt.pivot;

        var dt_pivot = Vector2.one / 2 - pivot;
        var dt_pos = new Vector2(rect.x * dt_pivot.x, rect.y * dt_pivot.y);
        var centerPoint = rt.TransformPoint(dt_pos);
        
        return centerPoint;
    }
    
    public static string GetDataPathDependOnPlatform(bool useStreamingPathInEditor, bool useStreamingPathInBuild)
    {
#if UNITY_EDITOR
        return useStreamingPathInEditor ? Application.streamingAssetsPath : Application.dataPath;
#else
        return useStreamingPathInBuild ? Application.streamingAssetsPath : Application.persistentDataPath;
#endif
    }
    
    public static string GetPlatformName()
    {
#if UNITY_EDITOR
        return GetPlatformForAssetBundles(EditorUserBuildSettings.activeBuildTarget);
#else
        return GetPlatformForAssetBundles(Application.platform);
#endif
    }

#if UNITY_EDITOR
    private static string GetPlatformForAssetBundles(BuildTarget target)
    {
        switch (target)
        {
            case BuildTarget.Android:
                return "Android";
            case BuildTarget.iOS:
                return "iOS";
            case BuildTarget.WebGL:
                return "WebGL";
            case BuildTarget.StandaloneWindows:
            case BuildTarget.StandaloneWindows64:
                return "Windows";
            case BuildTarget.StandaloneOSX:
                return "OSX";
            default:
                return null;
        }
    }
#endif
    
    private static string GetPlatformForAssetBundles(RuntimePlatform platform)
    {
        switch (platform)
        {
            case RuntimePlatform.Android:
                return "Android";
            case RuntimePlatform.IPhonePlayer:
                return "iOS";
            case RuntimePlatform.WebGLPlayer:
                return "WebGL";
            case RuntimePlatform.WindowsPlayer:
                return "Windows";
            case RuntimePlatform.OSXPlayer:
                return "OSX";
            default:
                return null;
        }
    }
    
    public static void FreeMemory()
    {
        Resources.UnloadUnusedAssets();
    }
    
    #endregion
}
