using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

[CustomEditor(typeof(PrefabLightmapBaker))]
public class PrefabLightmapBakerEditor : Editor
{
    private static GameObject[] selectedPrefabs;
    
    [MenuItem("RKStudio/Lightmap/Bake Prefab Lightmaps")]
    static void GenerateLightmapInfo()
    {
        selectedPrefabs = GetSelectedPrefabs();
        if (selectedPrefabs == null) return;
        
        BakePrefabLightmaps();
    }

    static GameObject[] GetSelectedPrefabs()
    {
        var selectedPrefabs = Selection.gameObjects;
        if (selectedPrefabs == null || selectedPrefabs.Length == 0)
        {
            EditorUtility.DisplayDialog("Generate lightmaps info", "Error! Please select prefabs to bake", "Ok");
            CLog.LogErrorOnceEditor($"[{nameof(PrefabLightmapBakerEditor)}-[{nameof(GenerateLightmapInfo)}] Error! Please select prefabs to bake.");
            return null;
        }

        return selectedPrefabs;
    }
    
    static List<PrefabLightmapBaker> CollectLightmapBakers(GameObject[] selectedPrefabs)
    {
        var prefabLightmapBakers = new List<PrefabLightmapBaker>();
        for (var i = 0; i < selectedPrefabs.Length; i++)
        {
            if (selectedPrefabs[i] == null) continue;
            var lightmapBaker = selectedPrefabs[i].GetComponent<PrefabLightmapBaker>();
            if (lightmapBaker == null) lightmapBaker = selectedPrefabs[i].AddComponent<PrefabLightmapBaker>();
            prefabLightmapBakers.Add(lightmapBaker);
        }

        return prefabLightmapBakers;
    }

    static async void BakePrefabLightmaps()
    {
        var lightingSettings = Lightmapping.lightingSettings;
        if (lightingSettings.autoGenerate)
        {
            EditorUtility.DisplayDialog("Generate lightmaps info", "Error! Please bake lightmaps first and disable Auto-Generate mode.", "Ok");
            CLog.LogErrorOnceEditor($"[{nameof(PrefabLightmapBakerEditor)}-{nameof(BakePrefabLightmaps)}] Error: Please bake lightmaps first and disable Auto-Generate mode.");
            return;
        }

        Lightmapping.bakeCompleted -= OnBakeComplete;
        Lightmapping.bakeCompleted += OnBakeComplete;
        if (!Lightmapping.BakeAsync())
        {
            EditorUtility.DisplayDialog("Generate lightmaps info", "Error! Can't start the bake.", "Ok");
            CLog.LogErrorOnceEditor($"[{nameof(PrefabLightmapBakerEditor)}-{nameof(BakePrefabLightmaps)}] Error! Can't start the bake.");
            return;
        }

        while (Lightmapping.isRunning)
        {
            var buildProcess = Lightmapping.buildProgress / 1f;
            EditorUtility.DisplayProgressBar("Baking prefabs lightmaps", $"Process: {(buildProcess * 100f):F2}% ", buildProcess);
            await Task.Yield();
        }
    }

    static void OnBakeComplete()
    {
        EditorUtility.ClearProgressBar();
        Lightmapping.bakeCompleted -= OnBakeComplete;
        
        var prefabLightmapBakers = CollectLightmapBakers(selectedPrefabs);
        SaveLightmapInfoToPrefabs(prefabLightmapBakers);
        
        EditorUtility.DisplayDialog("Generate lightmaps info", "Done baking prefab lightmaps!", "Ok");
        CLog.LogInfoLoopEditor($"[{nameof(PrefabLightmapBakerEditor)}-{nameof(OnBakeComplete)}] Done baking prefab lightmaps.");
    }
    
    static void SaveLightmapInfoToPrefabs(List<PrefabLightmapBaker> prefabLightmapBakers)
    {
        foreach (var lightmapBaker in prefabLightmapBakers)
        {
            var gameObject = lightmapBaker.gameObject;
            var lightmapInfos = new List<LightmapInfo>();
            var lightmaps = new List<Texture2D>();
            var directionalLightmaps = new List<Texture2D>();
            var shadowMasks = new List<Texture2D>();

            GenerateLightmapInfo(gameObject, lightmapInfos, lightmaps, directionalLightmaps, shadowMasks);

            lightmapBaker.LightmapInfo = lightmapInfos.ToArray();
            lightmapBaker.Lightmaps = lightmaps.ToArray();
            lightmapBaker.DirectionalLightmaps = directionalLightmaps.ToArray();
            lightmapBaker.ShadowMasks = shadowMasks.ToArray();

            var targetPrefab = PrefabUtility.GetCorrespondingObjectFromOriginalSource(lightmapBaker.gameObject);
            if (targetPrefab == null) continue;
            var root = PrefabUtility.GetOutermostPrefabInstanceRoot(lightmapBaker.gameObject);
            if (root != null)
            {
                var rootPrefab = PrefabUtility.GetCorrespondingObjectFromSource(lightmapBaker.gameObject);
                var rootPath = AssetDatabase.GetAssetPath(rootPrefab);
                PrefabUtility.UnpackPrefabInstanceAndReturnNewOutermostRoots(root, PrefabUnpackMode.OutermostRoot);
                try
                {
                    PrefabUtility.ApplyPrefabInstance(lightmapBaker.gameObject, InteractionMode.AutomatedAction);
                }
                catch
                {
                    
                }
                finally
                {
                    PrefabUtility.SaveAsPrefabAssetAndConnect(root, rootPath, InteractionMode.AutomatedAction);
                }
            }
            else PrefabUtility.ApplyPrefabInstance(lightmapBaker.gameObject, InteractionMode.AutomatedAction);
        }
    }

    static void GenerateLightmapInfo(GameObject root, List<LightmapInfo> rendererInfos, List<Texture2D> lightmaps, List<Texture2D> lightmapsDir, List<Texture2D> shadowMasks)
    {
        var renderers = root.GetComponentsInChildren<MeshRenderer>();
        foreach (var renderer in renderers)
        {
            if (renderer.lightmapIndex == -1) continue;
            var info = new LightmapInfo { renderer = renderer };

            if (renderer.lightmapScaleOffset == Vector4.zero) continue;
            if (renderer.lightmapIndex < 0 || renderer.lightmapIndex == 0xFFFE) continue;
            info.lightmapOffsetScale = renderer.lightmapScaleOffset;

            var lightmapIndex = renderer.lightmapIndex;
            var lightmap = LightmapSettings.lightmaps[lightmapIndex].lightmapColor;
            var lightmapDir = LightmapSettings.lightmaps[lightmapIndex].lightmapDir;
            var shadowMask = LightmapSettings.lightmaps[lightmapIndex].shadowMask;

            info.lightmapIndex = lightmaps.IndexOf(lightmap);
            if (info.lightmapIndex == -1)
            {
                info.lightmapIndex = lightmaps.Count;
                lightmaps.Add(lightmap);
                lightmapsDir.Add(lightmapDir);
                shadowMasks.Add(shadowMask);
            }

            rendererInfos.Add(info);
        }
    }
}
