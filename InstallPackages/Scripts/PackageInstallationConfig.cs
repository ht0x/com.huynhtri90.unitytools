using UnityEngine;
using System.IO;

public class PackageInstallationConfig
{
	#region ----- Variables ----
        
	internal const string PACKAGE_INSTALLATION_JSON_DATA_RESOURCE_FILE_NAME = "trihd_package_installation";
	internal const string GIT_GIST_FRONT_END_PROTOCOL = "https";
	internal const string GIT_GIST_BASE_ADDRESS = "gist.github.com";
	internal const string GIT_GIST_RAW_END_POINT = "raw";
	
	internal static readonly string PROJECT_MANIFEST_JSON_PATH = Path.Combine(Application.dataPath, "../Packages/manifest.json");
	
    #endregion
}
