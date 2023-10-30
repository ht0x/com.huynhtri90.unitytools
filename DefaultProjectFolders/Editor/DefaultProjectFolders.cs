using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

namespace HuynhTri
{
    public static class DefaultProjectFolders
    {
    	#region ----- Variables ----

        private const string FOLDERS_JSON_DATA_RESOURCE_FILE = "folders"; 
        
        #endregion
        
        #region ----- Main Methods -----

        public static bool CreateDirectories()
        {
            var data = LoadFoldersJsonData();
            if (ReferenceEquals(data, null)) return false;
            
            var rootPath = Application.dataPath;
            CreateDirectory(rootPath, data);
            
            return true;
        }

        private static void CreateDirectory(string path, Dictionary<string, object> data)
        {
            foreach (var pair in data)
            {
                var key = pair.Key;
                var value = pair.Value;
                var tempPath = Path.Combine(path, key);

                if (value is Dictionary<string, object> dicData)
                    CreateDirectory(tempPath, dicData);
                else if (value is null) Directory.CreateDirectory(tempPath);
                else if (value is string strValue)
                {
                    if (!string.IsNullOrEmpty(strValue)) tempPath = Path.Combine(tempPath, strValue); 
                    Directory.CreateDirectory(tempPath); 
                }
            }
        }

        private static Dictionary<string, object> LoadFoldersJsonData()
        {
            var textAsset = Resources.Load<TextAsset>(FOLDERS_JSON_DATA_RESOURCE_FILE);
            if (textAsset == null)
            {
                Debug.LogError($"[DefaultProjectFolders-LoadFoldersJsonData] Error! Can't file json data file with name: {FOLDERS_JSON_DATA_RESOURCE_FILE}");
                return null;
            }

            if (string.IsNullOrEmpty(textAsset.text))
            {
                Debug.LogError($"[DefaultProjectFolders-LoadFoldersJsonData] Json data is null or empty.");
                return null;
            }

            var data = Core.MiniJSON.Json.Deserialize(textAsset.text) as Dictionary<string, object>;
            if (ReferenceEquals(data, null))
            {
                Debug.LogError($"[DefaultProjectFolders-LoadFoldersJsonData] Can't convert json data to dictionary.");
                return null;
            }
            
            return data;
        }
        
        #endregion
    }
}
