
using System.Collections.Generic;
using UnityEngine;

public static class CLog
{
     #region ----- Enum -----

    public enum LogDependentCompilation
    {
        UnityEditor,
        IAP,
        Build_Test
    }

    public enum LogMode
    {
        All,
        OnlyErrors
    }
    
    public enum LogType
    {
        Info,
        Warning,
        Error
    }

    public enum LogLimit
    {
        NoLimit,
        OnlyOnce
    }
    
    #endregion
    
    #region ----- Variables -----
    
    private static LogMode logMode = LogMode.All;
    private static HashSet<string> _cachedLogMsgs = new HashSet<string>();
    
    #endregion
    
    #region ----- Properties -----
    
    /*
     * Info Logs
     */
    public static void LogInfoOnceEditor(string msg, params object[] objs) => Log(LogDependentCompilation.UnityEditor, LogType.Info, LogLimit.OnlyOnce, msg, objs);
    public static void LogInfoLoopEditor(string msg, params object[] objs) => Log(LogDependentCompilation.UnityEditor, LogType.Info, LogLimit.NoLimit, msg, objs);
    public static void LogInterpolationInfoOnceEditor(string className, string methodName, string msg, params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.UnityEditor, LogType.Info, LogLimit.OnlyOnce, msg, objs);
    }
    public static void LogInterpolationInfoLoopEditor(string className, string methodName, string msg, params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.UnityEditor, LogType.Info, LogLimit.NoLimit, msg, objs);
    }
    
    public static void LogInfoOnce(string msg, params object[] objs) => Log(LogDependentCompilation.Build_Test, LogType.Info, LogLimit.OnlyOnce, msg, objs);
    public static void LogInfoLoop(string msg, params object[] objs) => Log(LogDependentCompilation.Build_Test, LogType.Info, LogLimit.NoLimit, msg, objs);
    public static void LogInterpolationInfoOnce(string className, string methodName, string msg, params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.Build_Test, LogType.Info, LogLimit.OnlyOnce, msg, objs);
    }
    public static void LogInterpolationInfoLoop(string className, string methodName, string msg, params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.Build_Test, LogType.Info, LogLimit.NoLimit, msg, objs);
    }
    
    /*
     * Warning Logs
     */
    public static void LogWarningOnceEditor(string msg, params object[] objs) => Log(LogDependentCompilation.UnityEditor, LogType.Warning, LogLimit.OnlyOnce, msg, objs);
    public static void LogWarningLoopEditor(string msg, params object[] objs) => Log(LogDependentCompilation.UnityEditor, LogType.Warning, LogLimit.NoLimit, msg, objs);
    public static void LogInterpolationWarningOnceEditor(string className, string methodName, string msg, params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.UnityEditor, LogType.Warning, LogLimit.OnlyOnce, msg, objs);
    }
    public static void LogInterpolationWarningLoopEditor(string className, string methodName, string msg, params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.UnityEditor, LogType.Warning, LogLimit.NoLimit, msg, objs);
    }
    
    /*
     * Error Logs
     */
    public static void LogErrorOnceEditor(string msg, params object[] objs) => Log(LogDependentCompilation.UnityEditor, LogType.Error, LogLimit.OnlyOnce, msg, objs);
    public static void LogErrorLoopEditor(string msg, params object[] objs) => Log(LogDependentCompilation.UnityEditor, LogType.Error, LogLimit.NoLimit, msg, objs);

    public static void LogInterpolationErrorOnceEditor(string className, string methodName, string msg,
        params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.UnityEditor, LogType.Error, LogLimit.OnlyOnce, msg, objs);
    }
    public static void LogInterpolationErrorLoopEditor(string className, string methodName, string msg,
        params object[] objs)
    {
        var template = $"[{className}-{methodName}] ";
        msg = template + msg;
        Log(LogDependentCompilation.UnityEditor, LogType.Error, LogLimit.NoLimit, msg, objs);
    }
    
    public static void LogErrorOnce(string msg, params object[] objs) => Log(LogDependentCompilation.Build_Test, LogType.Error, LogLimit.OnlyOnce, msg, objs);
    public static void LogErrorLoop(string msg, params object[] objs) => Log(LogDependentCompilation.Build_Test, LogType.Error, LogLimit.NoLimit, msg, objs);

    public static void LogInterpolationErrorOnce(string className, string methodName, string msg,
        params object[] objs)
    {
        var template = $"[{className}-{methodName}]";
        msg = template + msg;
        Log(LogDependentCompilation.Build_Test, LogType.Error, LogLimit.OnlyOnce, msg, objs);
    }
    public static void LogInterpolationErrorLoop(string className, string methodName, string msg,
        params object[] objs)
    {
        var template = $"[{className}-{methodName}] ";
        msg = template + msg;
        Log(LogDependentCompilation.Build_Test, LogType.Error, LogLimit.NoLimit, msg, objs);
    }

    #endregion
    
    #region ----- Static Methods -----

    public static void Log(LogDependentCompilation logDependentCompilation, LogType logType, LogLimit logLimit, string msg, params object[] objs)
    {
        
#if !ENABLE_CUSTOM_LOG
        return;
#endif
        
        switch (logDependentCompilation)
        {
            case LogDependentCompilation.UnityEditor:
#if UNITY_EDITOR
                PrintLog(logType, logLimit, msg, objs);
#endif
                break;
            
            case LogDependentCompilation.IAP:
#if DEBUG_IAP
                PrintLog(logType, logLimit, msg, objs);
#endif
                break;

            case LogDependentCompilation.Build_Test:
#if LOG_BUILD_TEST
                PrintLog(logType, logLimit, msg, objs);
#endif
                break;
        }
    }
    
    static void PrintLog(LogType logType, LogLimit logLimit, string msg, params object[] objs)
    {
        var fullMsg = objs != null && objs.Length > 0 ? string.Format(msg, objs) : msg;
        switch (logLimit)
        {
            case LogLimit.OnlyOnce:
                if (_cachedLogMsgs.Contains(fullMsg))
                    return;
                break;
            
            case LogLimit.NoLimit:
                break;
        }
        
        if (logLimit == LogLimit.OnlyOnce && _cachedLogMsgs.Contains(fullMsg))
            return;

        _cachedLogMsgs.Add(fullMsg);
        switch (logMode)
        {
            case LogMode.All:
                if (logType == LogType.Info)
                    Debug.Log(fullMsg);
                else 
                if (logType == LogType.Warning)
                        Debug.LogWarning(fullMsg);
                else 
                if (logType == LogType.Error)
                        Debug.LogError(fullMsg);
                break;
            
            case LogMode.OnlyErrors:
                if (logType == LogType.Error)
                    Debug.LogError(fullMsg);
                break;
        }
    }
    
    #endregion
    
    #region ----- Methods -----

    public static void DestroyOnExit()
    {
        _cachedLogMsgs?.Clear();
    }
        
    #endregion
}
