using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class UserCache
{
    public static void SetInt(string key, int value) => PlayerPrefs.SetInt(key, value);
    public static int GetInt(string key) => PlayerPrefs.GetInt(key, 0);

    public static void SetBool(string key, bool value) => PlayerPrefs.SetInt(key, value ? 1 : 0);      
    public static bool GetBool(string key) => PlayerPrefs.GetInt(key, 0) == 1;

    public static void SetString(string key, string value) => PlayerPrefs.SetString(key, value);
    public static string GetString(string key) => PlayerPrefs.GetString(key, string.Empty);

    public static void SetFloat(string key, float value) => PlayerPrefs.SetFloat(key, value);
    public static float GetFloat(string key) => PlayerPrefs.GetFloat(key, 0);
    
    public static void Save() => PlayerPrefs.Save();
    public static void ClearSaves() => PlayerPrefs.DeleteAll();  
    public static void ClearSpecificSave(string saveKey) => PlayerPrefs.DeleteKey(saveKey);
}