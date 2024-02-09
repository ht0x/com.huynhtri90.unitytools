using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;

namespace HuynhTri
{
    public static class ToolMenuEditor
    {
        #region ----- Default Project Folders -----
        
        [MenuItem("Tools/TriHD/Setup Project/Create Default Folders")]
        static void CreateDefaultFolders()
        {
            if (!DefaultProjectFolders.CreateDirectories())
            {
                EditorUtility.DisplayDialog("Create default project folders",
                    "Can't create default folders! See console for more information.", "Ok");

                return;
            }

            EditorUtility.DisplayDialog("Create default project folders",
                "Done!", "Ok");
        }
        
        #endregion
        
        #region ----- Custom Script Templates -----

        [MenuItem("Tools/TriHD/Setup Project/Update script templates")]
        static void UpdateScriptTemplates()
        {
            EditorUtility.DisplayProgressBar("Update script templates", "Updating", 1f);
            var result = CustomScriptTemplates.UpdateTemplates();
            EditorUtility.ClearProgressBar();

            var msg = result ? "Done!" : "Can't update script templates. Please see console.";
            EditorUtility.DisplayDialog("Update script templates", msg, "Ok");
        }
        
        #endregion
        
        #region ----- Install Packages (TRIHD) -----

        [MenuItem("Tools/TriHD/Load New Project Manifest/2D Urp")]
        static async Task Load_2D_Urp_Manifest()
        {
            EditorUtility.DisplayProgressBar("Load project manifest", "Loading", 1f);
            var result = await PackageInstallation.LoadProjectManifestFromGist();
            EditorUtility.ClearProgressBar();

            var msg = result ? "Done!" : "Can't load 2d urp project manifest. Please see console.";
            EditorUtility.DisplayDialog("Load project manifest", msg, "Ok");
        }
        
        #endregion
        
        #region ----- Install Registry Package (Unity) -----
        
        [MenuItem("Tools/TriHD/Install Package/JetBrain Rider Editor")]
        static async Task InstallJetBrainRiderEditor()
        {
            EditorUtility.DisplayProgressBar("Install JetBrain Rider Editor", "Installing", 1f);
            var result = await PackageInstallation.InstallUnityPackage("ide.rider");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install JetBrain Rider Editor. Please see console.";
            EditorUtility.DisplayDialog("Install JetBrain Rider Editor", msg, "Ok");
        }
        
        [MenuItem("Tools/TriHD/Install Package/Unity New Input System")]
        static async Task InstallUnityInputSystem()
        {
            EditorUtility.DisplayProgressBar("Install unity new input system", "Installing", 1f);
            var result = await PackageInstallation.InstallUnityPackage("inputsystem");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install unity new input system. Please see console.";
            EditorUtility.DisplayDialog("Install unity new input system", msg, "Ok");
        }
        
        [MenuItem("Tools/TriHD/Install Package/FBX Exporter")]
        static async Task InstallFBXExporter()
        {
            EditorUtility.DisplayProgressBar("Install FBX Exporter", "Installing", 1f);
            var result = await PackageInstallation.InstallUnityPackage("formats.fbx");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install FBX Exporter. Please see console.";
            EditorUtility.DisplayDialog("Install FBX Exporter", msg, "Ok");
        }
        
        [MenuItem("Tools/TriHD/Install Package/AR Bundles")]
        static async Task InstallARBundles()
        {
            EditorUtility.DisplayProgressBar("Install AR bundles", "Installing", 1f);
            var result = await PackageInstallation.InstallUnityPackage("feature.ar");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install AR bundles. Please see console.";
            EditorUtility.DisplayDialog("Install AR bundles", msg, "Ok");
        }
        
        [MenuItem("Tools/TriHD/Install Package/VR Bundles")]
        static async Task InstallVRBundles()
        {
            EditorUtility.DisplayProgressBar("Install VR bundles", "Installing", 1f);
            var result = await PackageInstallation.InstallUnityPackage("feature.vr");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install VR bundles. Please see console.";
            EditorUtility.DisplayDialog("Install VR bundles", msg, "Ok");
        }
        
        [MenuItem("Tools/TriHD/Install Package/Addressables")]
        static async Task InstallAddressables()
        {
            EditorUtility.DisplayProgressBar("Install Addressables", "Installing", 1f);
            var result = await PackageInstallation.InstallUnityPackage("addressables");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install Addressables. Please see console.";
            EditorUtility.DisplayDialog("Install Addressables", msg, "Ok");
        }
        
        #endregion
        
        #region ----- Install External Package -----
        
        [MenuItem("Tools/TriHD/Install Package/UniTask")]
        static async Task InstallUniTask()
        {
            EditorUtility.DisplayProgressBar("Install UniTask", "Installing", 1f);
            var result = await PackageInstallation.InstallExternalPackage("com.cysharp.unitask");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install UniTask. Please see console.";
            EditorUtility.DisplayDialog("Install UniTask", msg, "Ok");
        }
        
        #endregion
    }
}
