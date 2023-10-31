using System.IO;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

public static class PackageInstallation
{
	#region ----- Variables ----

	private const string GIT_GIST_FRONT_END_PROTOCOL = "https";
	private const string GIT_GIST_BASE_ADDRESS = "gist.github.com";
	private static readonly string PROJECT_MANIFEST_JSON_PATH = Path.Combine(Application.dataPath, "../Packages/manifest.json");
	
    #endregion
    
    #region ----- Main Methods -----

    public static async Task<bool> InstallPackage(string packageName)
    {
	    var addRequest = Client.Add($"com.unity.{packageName}");
	    while (!addRequest.IsCompleted || addRequest.Status == StatusCode.InProgress)
		    await Task.Yield();

	    var result = addRequest.Status == StatusCode.Success;
	    if (!result) 
		    Debug.LogError($"[PackageInstallation-InstallPackage] Unable to install package name: {packageName}." +
		                   $"\nStatus code: {addRequest.Status}" +
		                   $"\nError code: {addRequest.Error.errorCode}" +
		                   $"\nError msg: {addRequest.Error.message}");
	    
	    return result;
    }
    
    public static async Task<bool> LoadProjectManifestFromGist(string gistUserName, string gistId)
    {
	    var gistUrl = GetGistUrl(gistUserName, gistId);
	    var packageJsonContent = await GetPackageJsonContent(gistUrl);
	    if (string.IsNullOrEmpty(packageJsonContent)) 
		    return false;
	    
	    ReplacePackageFile(PROJECT_MANIFEST_JSON_PATH, packageJsonContent);
	    return true;
    }

    static void ReplacePackageFile(string path, string newContent)
    {
	    File.WriteAllText(path, newContent);
	    Client.Resolve();
    }
    
    #endregion
    
    #region ----- Utility Methods -----

    static async Task<string> GetPackageJsonContent(string url)
    {
	    using var httpRequest = new HttpClient();
	    var response = await httpRequest.GetAsync(url);
	    if (!response.IsSuccessStatusCode)
	    {
		    Debug.LogError($"[PackageInstallation-GetPackageJsonContent] Failed to get response.\nUrl: {url}\nStatus Code: {(int)response.StatusCode} - {response.StatusCode.ToString()}");
		    return string.Empty;
	    }

	    var content = await response.Content.ReadAsStringAsync();
	    return content;
    }
    
    static string GetGistUrl(string user, string id)
    {
	    return GIT_GIST_FRONT_END_PROTOCOL + "://" + GIT_GIST_BASE_ADDRESS + "/" + user + "/" + id;
    }
    
    #endregion
}
