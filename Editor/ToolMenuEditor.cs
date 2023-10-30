using UnityEditor;

namespace HuynhTri
{
    public static class ToolMenuEditor
    {
        [MenuItem("Tools/Setup Project/Create Default Folders")]
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
    }
}
