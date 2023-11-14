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
        
        #region ----- Install Package (Unity) -----
        
        [MenuItem("Tools/TriHD/Install Package/Unity New Input System")]
        static async Task InstallUnityInputSystem()
        {
            EditorUtility.DisplayProgressBar("Install unity new input system", "Installing", 1f);
            var result = await PackageInstallation.InstallPackage("inputsystem");
            EditorUtility.ClearProgressBar();
            
            var msg = result ? "Done!" : "Can't install unity new input system. Please see console.";
            EditorUtility.DisplayDialog("Install unity new input system", msg, "Ok");
        }
        
        #endregion
    }
}
