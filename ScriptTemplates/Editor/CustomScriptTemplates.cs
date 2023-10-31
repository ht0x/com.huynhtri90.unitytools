using System.IO;
using UnityEditor;
using UnityEngine;

public static class CustomScriptTemplates
{
	#region ----- Variables ----

	private const string ASSET_SCRIPT_TEMPLATES_FOLDER_NAME = "ScriptTemplates";
	private const string SCRIPT_TEMPLATES_POOL_RESOURCES_PATH = "trihd_script_templates";
	
    #endregion
    
    #region ----- Main Methods -----

    public static bool UpdateTemplates()
    {
	    var path = Path.Combine(Application.dataPath, ASSET_SCRIPT_TEMPLATES_FOLDER_NAME);
	    CreateTemplateFolder(path);
	    var templatePaths = GetTemplatesPaths(SCRIPT_TEMPLATES_POOL_RESOURCES_PATH);
	    if (templatePaths == null)
	    {
		    Debug.Log($"[CustomScriptTemplates-UpdateTemplates] Templates pool is null or empty with resource path: {SCRIPT_TEMPLATES_POOL_RESOURCES_PATH}");
		    return false;
	    }

	    CopyTemplatesToLocation(templatePaths, path);
	    AssetDatabase.Refresh();
	    return true;
    }

    static void CreateTemplateFolder(string path)
    {
	    Directory.CreateDirectory(path);
    }

    static string[] GetTemplatesPaths(string resourcePath)
    {
	    var templatesPool = Resources.LoadAll<TextAsset>(resourcePath);
	    if (templatesPool == null || templatesPool.Length == 0) return null;

	    var poolPaths = new string[templatesPool.Length];
	    for (var i = 0; i < templatesPool.Length; i++)
		    poolPaths[i] = AssetDatabase.GetAssetPath(templatesPool[i]);

	    return poolPaths;
    }

    static void CopyTemplatesToLocation(string[] templatePaths, string destination)
    {
	    for (var i = 0; i < templatePaths.Length; i++)
	    {
		    var templatePath = templatePaths[i];
		    var fileNameWithExtension = Path.GetFileName(templatePath);
		    var newLocation = Path.Combine(destination, fileNameWithExtension).Replace(Application.dataPath, "Assets");
		    
		    if (!AssetDatabase.CopyAsset(templatePath, newLocation))
				Debug.LogError($"[CustomScriptTemplate-CopyTemplatesToLocation] Can't copy asset with path: {templatePath}\nDestination: {newLocation}");
	    }
    }
    
    #endregion
}
