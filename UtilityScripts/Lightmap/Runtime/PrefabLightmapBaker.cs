using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class PrefabLightmapBaker : MonoBehaviour
{
    #region  ----- Variables -----
    
    [SerializeField]
    private LightmapInfo[] lightmapInfo;
    [SerializeField]
    private Texture2D[] lightmaps;
    [SerializeField]
    private Texture2D[] directionalLightmaps;
    [SerializeField]
    private Texture2D[] shadowMasks;
    
    private int[] offsetLightmapIndexes;
    private LightmapData[] combinedLightmaps;
    
    #endregion
    
    #region ----- Properties -----

    public LightmapInfo[] LightmapInfo
    {
        get => lightmapInfo;
        set => lightmapInfo = value;
    }
    
    public Texture2D[] Lightmaps  
    {
        get => lightmaps;
        set => lightmaps = value;
    }
    
    public Texture2D[] DirectionalLightmaps  
    {
        get => directionalLightmaps;
        set => directionalLightmaps = value;
    }
    
    public Texture2D[] ShadowMasks  
    {
        get => shadowMasks;
        set => shadowMasks = value;
    }

    #endregion
    
    #region ----- Unity Methods -----
    
    void Awake()
    {
        Init();
    }

    #endregion
    
    #region ----- Methods -----
    
    void Init()
    {
        CombineLightmaps();
        InitLightmapSetting();
    }
    
    void InitLightmapSetting()
    {
        var directional = directionalLightmaps.All(dirTex2d => dirTex2d != null);
        ApplyLightmapInfo(lightmapInfo, offsetLightmapIndexes);
        LightmapSettings.lightmapsMode = (directionalLightmaps.Length == lightmaps.Length && directional) ? LightmapsMode.CombinedDirectional : LightmapsMode.NonDirectional;
        LightmapSettings.lightmaps = combinedLightmaps.ToArray();
    }

    void CombineLightmaps()
    {
        if (lightmapInfo == null || lightmapInfo.Length == 0)
            return;

        var lightmapsFromSettings = LightmapSettings.lightmaps;
        var lightmapCount = lightmapsFromSettings.Length;
        offsetLightmapIndexes = new int[lightmaps.Length];
        var tempLightmaps = new List<LightmapData>();

        for (var i = 0; i < lightmaps.Length; i++)
        {
            var exists = false;
            for (var j = 0; j < lightmapsFromSettings.Length; j++)
            {
                if (lightmaps[i] != lightmapsFromSettings[j].lightmapColor) continue;
                exists = true;
                offsetLightmapIndexes[i] = j;
            }

            if (exists) continue;
            offsetLightmapIndexes[i] = lightmapCount;
            var newLightmapData = new LightmapData
            {
                lightmapColor = lightmaps[i],
                lightmapDir = directionalLightmaps.Length == lightmaps.Length ? directionalLightmaps[i] : default,
                shadowMask = shadowMasks.Length == lightmaps.Length  ? shadowMasks[i] : default,
            };

            tempLightmaps.Add(newLightmapData);
            lightmapCount += 1;
        }

        combinedLightmaps = new LightmapData[lightmapCount];
        lightmapsFromSettings.CopyTo(combinedLightmaps, 0);
        tempLightmaps.ToArray().CopyTo(combinedLightmaps, lightmapsFromSettings.Length);
    }

    static void ApplyLightmapInfo(LightmapInfo[] infos, int[] lightmapOffsetIndex)
    {
        foreach (var info in infos)
        {
            info.renderer.lightmapIndex = lightmapOffsetIndex[info.lightmapIndex];
            info.renderer.lightmapScaleOffset = info.lightmapOffsetScale;
            var mat = info.renderer.sharedMaterials;
            foreach (var t in mat)
            {
                if (t != null && Shader.Find(t.shader.name) != null)
                    t.shader = Shader.Find(t.shader.name);
            }
        }
    }
    
    #endregion
}
