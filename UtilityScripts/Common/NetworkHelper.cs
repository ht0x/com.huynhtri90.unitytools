using UnityEngine;
using System;
using System.Collections;
using EnumCollection;

public class NetworkHelper : MonoBehaviour
{
    #region ----- Variables -----

    private bool isConnected;
    private static NetworkHelper s_instance = null;
    [SerializeField]
    private float retryIntervalWhenSucceeded = 6f;
    [SerializeField]
    private float retryIntervalWhenFailed = 4f;    

    #endregion

    #region ----- Properties -----

    public static NetworkHelper Instance
    {
        get
        {
            if (s_instance == null)
            {             
                var go = new GameObject
                {
                    name = "NetworkHelper"
                };
                
                s_instance = go.AddComponent<NetworkHelper>();
            }
            
            return s_instance;
        }
    }

    public bool IsConnected => isConnected && Application.internetReachability != NetworkReachability.NotReachable;

    #endregion

    #region ----- Unity Methods -----

    void Awake()
    {
        if (s_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        
        s_instance = this;
        //DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        //StartCoroutine(_PingLoop());
    }

    #endregion

    #region ----- Methods -----
  
    void CheckConnection(Action<INTERNET_CONNECTION_STATUS> callback, float timeout = 2f, int numOfRetries = 1)
    {
        StartCoroutine(_Ping(timeout, numOfRetries, callback));
    }

    public void OnConnectionRetry(Action<INTERNET_CONNECTION_STATUS> onRetryCallback, float timeout = 2f, int numOfRetries = 1)
    {       
        CheckConnection(onRetryCallback, timeout, numOfRetries);
    }

    public static bool HasNoConnection()
    {
        return Application.internetReachability == NetworkReachability.NotReachable;
    }

    private IEnumerator _PingLoop()
    {
        while (true)
        {
            if (Application.internetReachability == NetworkReachability.NotReachable)
            {
                isConnected = false;              
                yield return new WaitForSecondsRealtime(1f);
            }
            else
            {
                var ping = new Ping("8.8.8.8");
                var timeout = Time.time + 3;

                while (!ping.isDone && Time.time < timeout)
                    yield return null;

                isConnected = !(!ping.isDone || ping.time < 0);              
                yield return new WaitForSecondsRealtime(isConnected ? retryIntervalWhenSucceeded : retryIntervalWhenFailed);
            }
        }
    }

    private IEnumerator _Ping(float timeout, int numOfRetries, Action<INTERNET_CONNECTION_STATUS> action)
    {
        if (Application.internetReachability == NetworkReachability.NotReachable)
        {
            isConnected = false;
            action?.Invoke(INTERNET_CONNECTION_STATUS.NO_CONNECTION);
        }
        else
        {
            var startTime = Time.time;
            for (int i = 0; i < numOfRetries; i++)
            {
                var ping = new Ping("8.8.8.8");
                var nextTimeout = Time.time + timeout;
           
                while ((!ping.isDone) && Time.time < nextTimeout)
                    yield return null;
                
                isConnected = !(!ping.isDone || ping.time < 0);
                if (isConnected)
                    break;
            }

            var timePassed = Time.time - startTime;
            var waitingTimeLeft = timeout - timePassed;
            if ((timePassed >= 0f) && (timePassed < waitingTimeLeft))
                yield return new WaitForSeconds(waitingTimeLeft);
            
            action?.Invoke(isConnected ? INTERNET_CONNECTION_STATUS.CONNECTED : INTERNET_CONNECTION_STATUS.NO_CONNECTION);
        }
    }

    #endregion
}