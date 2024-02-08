using UnityEngine;
using System;
using System.IO;
using UnityEngine.Networking;
using System.Linq;
using System.Collections.Generic;

public partial class MyUtilities
{
    public static string GetValidName(string[] nameList, string nameToValidate)
    {
        if (!nameList.Contains(nameToValidate))
            return nameToValidate;

        string newName = nameToValidate;
        int postfixNumber = 1;

        while (nameList.Contains(newName))
        {
            if (newName.EndsWith($" ({postfixNumber})"))
                postfixNumber++;
            else
                newName = $"{nameToValidate} ({postfixNumber})";
        }

        return newName;
    }
    public static string GetFileOrFolderPath(string fileOrFolderName, bool useStreamingPathInEditor, bool useStreamingPathInBuild)
    {
        return Path.Combine(GetDataPathDependOnPlatform(useStreamingPathInEditor, useStreamingPathInBuild), fileOrFolderName);
    }

    public static void WriteDataToFile(string fileName, string data, bool useStreamingPathInEditor, bool useStreamingPathInBuild)
    {
        string filePath = GetFileOrFolderPath(fileName, useStreamingPathInEditor, useStreamingPathInBuild);
        using StreamWriter sw = File.CreateText(filePath);
        sw.Write(data);
    }

    public static string ReadDataFromFile(string fileName, bool useStreamingPathInEditor, bool useStreamingPathInBuild)
    {
        string output = string.Empty;
        try
        {
            string filePath = GetFileOrFolderPath(fileName, useStreamingPathInEditor, useStreamingPathInBuild);
            if (!ExistsFile(filePath))
                return output;

            output = ReadText(filePath);

            return output;
        }
        catch (Exception)
        {
            output = string.Empty;
            return output;
        }
    }

    public static bool IsPathFile(string path)
    {
        var extension = Path.GetExtension(path);
        return !string.IsNullOrEmpty(extension);
    }

    public static bool IsPathDirectory(string path)
    {
        return !IsPathFile(path);
    }

    public static bool ExistsFile(string fullPath)
    {
        if (fullPath.Contains("://"))
            return DoesFileExistInStreamingAssets(fullPath);
        else
            return File.Exists(fullPath);
    }

    public static bool ExistsDirectory(string fullPath)
    {
        if (fullPath.Contains("://"))
            return DoesDirectoryExistInStreamingAssets(fullPath);
        else
            return Directory.Exists(fullPath);
    }

    public static string[] GetDirectories(string directoryName)
    {
        string relativePath = Path.GetRelativePath(Application.streamingAssetsPath, directoryName);
        if (directoryName.Contains("://"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // On Android, the streaming assets files are compressed in the APK file, so we need to access them using AndroidJavaObject
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject assetManager = currentActivity.Call<AndroidJavaObject>("getAssets");

            string[] fileNames = assetManager.Call<string[]>("list", relativePath);
           
            List<string> fileDirs = new List<string>();
            for (int i = 0; i < fileNames.Length; i++)
            {
                var temp = Path.Combine(directoryName, fileNames[i]);
                if(IsPathDirectory(temp))
                 fileDirs.Add(temp);
            }
            return fileDirs.ToArray();
#else
            DirectoryInfo d = new DirectoryInfo(directoryName);
            var directories = d.GetDirectories();
            var dirs = directories.Select(x => x.FullName).ToArray();
            return dirs;
#endif
        }
        else
        {
            DirectoryInfo d = new DirectoryInfo(directoryName);
            var directories = d.GetDirectories();
            var dirs = directories.Select(x => x.FullName).ToArray();
            return dirs;
        }
    }


    public static List<string> GetFiles(string directoryName)
    {
        string relativePath = Path.GetRelativePath(Application.streamingAssetsPath, directoryName);
        if (directoryName.Contains("://"))
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // On Android, the streaming assets files are compressed in the APK file, so we need to access them using AndroidJavaObject
            AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject assetManager = currentActivity.Call<AndroidJavaObject>("getAssets");

            string[] fileNames = assetManager.Call<string[]>("list", relativePath);

            List<string> fileDirs = new List<string>();
            for (int i = 0; i < fileNames.Length; i++)
            {
                var temp = Path.Combine(directoryName, fileNames[i]);
                fileDirs.Add(temp);
            }
            return fileDirs;
#else
            var ItemDir = new DirectoryInfo(directoryName);
            return ItemDir.GetFiles().Select(x => x.FullName).ToList();
#endif
        }
        else { 
            var ItemDir = new DirectoryInfo(directoryName);
            return ItemDir.GetFiles().Select(x => x.FullName).ToList();
        }
    }

    public static string ReadText(string filename)
    {
        if (filename.Contains("://"))
        {
            UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(filename);
            www.SendWebRequest();
            while (!www.isDone) { }
            string contents = www.downloadHandler.text;
            return contents;
        }
        else
        {

            using (StreamReader sr = File.OpenText(filename))
            {
                string output = sr.ReadToEnd();
                return output;
            }
        }
       
    }

    public static byte[] ReadByte(string fileName)
    {
        byte[] bytes;
        if (fileName.Contains("://"))
        {
            // Load the file using UnityWebRequest if it's a remote file
            using var www = UnityWebRequest.Get(fileName);
            www.SendWebRequest();
            while (!www.isDone) { }
            bytes = www.downloadHandler.data;
        }
        else
        {
            using FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.Read);
            bytes = new byte[stream.Length];
            stream.Read(bytes, 0, bytes.Length);
        }
        
        return bytes;
    }

    #region Private Methods
    private static bool DoesFileExistInStreamingAssets(string fullPath)
    {
#if UNITY_ANDROID && !UNITY_EDITOR
            // On Android, streaming assets must be accessed using UnityWebRequest.
            using var www = UnityWebRequest.Get(fullPath);
            www.SendWebRequest();

            while (!www.isDone)
            {
            }

            if (www.result != UnityWebRequest.Result.Success)
            {
                CLog.LogInfoLoopEditor($"Could not open file at path {fullPath}: {www.error}");
                return false;
            }
            else 
                return true;
#else
        return File.Exists(fullPath);
#endif
    }

    private static bool DoesDirectoryExistInStreamingAssets(string directoryName)
    {
        bool directoryExists = false;

        string relativePath = Path.GetRelativePath(Application.streamingAssetsPath, directoryName);
#if UNITY_ANDROID && !UNITY_EDITOR
        // On Android, the streaming assets files are compressed in the APK file, so we need to access them using AndroidJavaObject
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        AndroidJavaObject assetManager = currentActivity.Call<AndroidJavaObject>("getAssets");

        string[] fileNames = assetManager.Call<string[]>("list", relativePath);
        if (fileNames.Length > 0)
            directoryExists = true;
#else
        directoryExists = Directory.Exists(directoryName);
#endif
        return directoryExists;
    }

    #endregion
}
