using System.Threading.Tasks;
using UnityEditor;

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
            
            AssetDatabase.Refresh();
        }
        
        #endregion
        
        #region ----- Install Packages (TRIHD) -----

        private const string GIT_GIST_TRIHD_USERNAME = "ht0x";
	    private const string GIT_GIST_TRIHD_2D_URP_PACKAGE_ID = "11c9dcb1c1d777524df86960234fd6ae";
        
        [MenuItem("Tools/TriHD/Load New Project Manifest/2D Urp")]
        static async Task Load_2D_Urp_Manifest()
        {
            EditorUtility.DisplayProgressBar("Load project manifest", "Loading", 1f);
            var result = await PackageInstallation.LoadProjectManifestFromGist(GIT_GIST_TRIHD_USERNAME, GIT_GIST_TRIHD_2D_URP_PACKAGE_ID);
            EditorUtility.ClearProgressBar();

            var msg = result ? "Done!" : "Can't load 2d urp project manifest. Please see console.";
            EditorUtility.DisplayDialog("Load project manifest", msg, "Ok");
        }
        
        #endregion
        
        #region ----- Install Package (Unity) -----
        
        // [MenuItem("Tools/TriHD/Install Package/DoTween")]
        // static void InstallDoTween()
        // {
        //     
        // }
        //
        // [MenuItem("Tools/TriHD/Install Package/UniTask")]
        // static void InstallUniTask()
        // {
        //     
        // }
        
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
