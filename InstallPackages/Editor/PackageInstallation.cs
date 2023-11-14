using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor.PackageManager;

namespace HuynhTri
{
	public static class PackageInstallation
	{
		#region ----- Inner Class -----

		private class PackageData
		{
			private string gitUsername;
			private Dictionary<string, string> git2dUrpDataMapping;

			internal string GitUsername => gitUsername;

			internal PackageData(Dictionary<string, object> data)
			{
				Load(data);
			}

			void Load(Dictionary<string, object> data)
			{
				if (ReferenceEquals(data, null)) return;
				if (data.TryGetValue("gitUsername", out var username))
					gitUsername = username.ToString();

				if (data.TryGetValue("gitGist2dUrpData", out var gist2dUrpData))
				{
					if (gist2dUrpData is Dictionary<string, object> dicData)
					{
						git2dUrpDataMapping = new Dictionary<string, string>();
						foreach (var pair in dicData)
							git2dUrpDataMapping.TryAdd(pair.Key, pair.Value.ToString());
					}
				}
			}

			internal string GetGistId(string unityVersion)
			{
				if (ReferenceEquals(git2dUrpDataMapping, null)) return string.Empty;
				return git2dUrpDataMapping.TryGetValue(unityVersion, out var foundGistId)
					? foundGistId.ToString()
					: string.Empty;
			}
		}

		#endregion

		#region ----- Variables ----

		private const string PACKAGE_INSTALLATION_JSON_DATA_RESOURCE_FILE_NAME = "trihd_package_installation";
		private const string GIT_GIST_FRONT_END_PROTOCOL = "https";
		private const string GIT_GIST_BASE_ADDRESS = "gist.github.com";
		private const string GIT_GIST_RAW_END_POINT = "raw";

		private static readonly string PROJECT_MANIFEST_JSON_PATH =
			Path.Combine(Application.dataPath, "../Packages/manifest.json");

		private static PackageData packageData;

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

		public static async Task<bool> LoadProjectManifestFromGist()
		{
			if (ReferenceEquals(packageData, null))
			{
				if (!LoadPackageData()) return false;
			}
			
			var gistUrl = GetGistUrl(packageData?.GitUsername, packageData?.GetGistId(Application.unityVersion));
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

		static bool LoadPackageData()
		{
			var textAsset = Resources.Load<TextAsset>(PACKAGE_INSTALLATION_JSON_DATA_RESOURCE_FILE_NAME);
			if (textAsset == null)
			{
				Debug.LogError($"[PackageInstallation-LoadPackageData] Can't load package installation json data with path: " +
				               $"{PACKAGE_INSTALLATION_JSON_DATA_RESOURCE_FILE_NAME}");
				return false;
			}

			var dicData = Core.MiniJSON.Json.Deserialize(textAsset.text) as Dictionary<string, object>;
			if (ReferenceEquals(dicData, null))
			{
				Debug.LogError($"[PackageInstallation-LoadPackageData] Unable to cast json data to dictionary type.");
				return false;
			}
			
			packageData = new PackageData(dicData);
			return true;
		}

		#endregion

		#region ----- Utility Methods -----

		static async Task<string> GetPackageJsonContent(string url)
		{
			using var httpRequest = new HttpClient();
			var response = await httpRequest.GetAsync(url);
			if (!response.IsSuccessStatusCode)
			{
				Debug.LogError(
					$"[PackageInstallation-GetPackageJsonContent] Failed to get response.\nUrl: {url}\nStatus Code: {(int)response.StatusCode} - {response.StatusCode.ToString()}");
				return string.Empty;
			}

			var content = await response.Content.ReadAsStringAsync();
			return content;
		}

		static string GetGistUrl(string user, string id)
		{
			return GIT_GIST_FRONT_END_PROTOCOL + "://" + GIT_GIST_BASE_ADDRESS + "/" + user + "/" + id + "/" +
			       GIT_GIST_RAW_END_POINT;
		}

		#endregion
	}
}
