using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T: MonoBehaviour
{
    #region ----- Variables -----

    private static T s_instance = null;
    private static object s_lock = new();
    private static bool isShutDown;

    #endregion

    #region ----- Properties -----

    public static T Instance
    {
        get
        {
            if (isShutDown)
            {
                CLog.LogErrorLoopEditor("[SingletonMonobehaviour] Instance '" + typeof(T) + "' already destroyed. Returning null.");
                return null;
            }

            lock (s_lock)
            {
                if (s_instance != null) return s_instance;
                s_instance = (T)FindObjectOfType(typeof(T));
                
                if (s_instance != null) return s_instance;
                
                var singletonObject = new GameObject();
                s_instance = singletonObject.AddComponent<T>();
                singletonObject.name = typeof(T).ToString() + " (Singleton)";

                return s_instance;
            }
        }
    }

    #endregion

    #region ----- Unity Methods -----

    protected virtual void Awake()
    {
        if (!ReferenceEquals(s_instance, null))
        {
            Destroy(gameObject);
            return;
        }

        s_instance = this as T;
    }

    protected virtual void OnApplicationQuit()
    {
        isShutDown = true;
        s_instance = null;
    }

    protected virtual void OnDestroy()
    {
        isShutDown = true;
        s_instance = null;
    }

    #endregion
}
