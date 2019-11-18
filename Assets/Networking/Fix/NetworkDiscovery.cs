using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public static class AvailableGamesList
{
    public static event Action<List<LanConnnectionInfo>> OnAvailableMatchesChanged = delegate { };

    private static List<LanConnnectionInfo> gamesList = new List<LanConnnectionInfo>();

    public static void HandleNewGamesList(List<LanConnnectionInfo> list)
    {
        gamesList = list;
        OnAvailableMatchesChanged(gamesList);
    }

}

public struct LanConnnectionInfo
{
    public const int PORT = 7750; // Port is fixed and defined in NetworkManager settings

    public string ipAddress;
    public string gameName; // 
    public string hostName;

    // Parses the info from the broadcasted data
    public LanConnnectionInfo(string fromAddress, string data)
    {
        ipAddress = fromAddress.Substring(fromAddress.LastIndexOf(':') + 1); ;

        string[] tokens = data.Split(';');
        gameName = tokens[0];
        hostName = tokens[1];
    }
}

public class ConnectionDiscovery : NetworkDiscovery
{
    private ConnectionDiscovery instance;

    public ConnectionDiscovery Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<ConnectionDiscovery>();

            return instance;
        }
    }


    private const float EXPIRE_TIMEOUT = 2f;

    private bool isBroadcasting = false;
    private bool isListening = false;

    private AndroidJavaObject multicastLock;

    private Dictionary<LanConnnectionInfo, float> lanAddresses = new Dictionary<LanConnnectionInfo, float>();

    /// <summary>
    /// Starts broadcasting the connection parameters.
    /// </summary>
    /// <param name="gameName">The name of the game to broadcast.</param>
    public void StartBroadcasting(string gameName, string hostName)
    {
        if (!isBroadcasting && !isListening)
        {
            if (Application.platform == RuntimePlatform.Android)
                MulticastLock();

            broadcastData = gameName + ";" + hostName;
            broadcastData = broadcastData.PadRight(20, '#');
            Initialize();
            StartAsServer();
            isBroadcasting = true;
        }
    }

    /// <summary>
    /// Starts listening for connection parameters.
    /// </summary>
    public void StartListening()
    {
        if (!isListening && !isBroadcasting)
        {
            if (Application.platform == RuntimePlatform.Android)
                MulticastLock();

            base.Initialize();
            base.StartAsClient();

            isListening = true;
            StartCoroutine(CleanupExpiredEntries());
        }
    }

    /// <summary>
    /// Stops all activity (broadcast and listening)
    /// </summary>
    public void Stop()
    {
        if (isBroadcasting || isListening)
        {
            if (multicastLock != null)
                multicastLock.Call("release");

            base.StopBroadcast();
            isBroadcasting = false;
            isListening = false;
            lanAddresses.Clear();
        }
    }

    /// <summary>
    /// Callback when new broadcastData is received.
    /// </summary>
    /// <param name="fromAddress">The address we receive from.</param>
    /// <param name="data">Additional data (game name)</param>
    //public override void OnReceivedBroadcast(string fromAddress, string data)
    //{
    //    base.OnReceivedBroadcast(fromAddress, data);
    //    Debug.Log("Received broadcast: " + fromAddress + ", " + data);

    //    data = data.Replace("#", ""); // Remove padded chars

    //    LanConnnectionInfo info = new LanConnnectionInfo(fromAddress, data);
    //    Debug.Log(info.gameName + ", " + info.hostName);

    //    if (!lanAddresses.ContainsKey(info))
    //    {
    //        lanAddresses.Add(info, Time.time + EXPIRE_TIMEOUT);
    //        UpdateMatchInfo();
    //    }
    //    else
    //    {
    //        lanAddresses[info] = Time.time + EXPIRE_TIMEOUT;
    //    }
    //}

    /// <summary>
    /// Used to clean up expired entries in our list of available games.
    /// Uses a timeout to delete it from the list if we don't receive the data again in the specified time window.
    /// </summary>
    /// <returns></returns>
    private IEnumerator CleanupExpiredEntries()
    {
        while (true)
        {
            bool changed = false;

            var keys = lanAddresses.Keys.ToList();
            foreach (var key in keys)
            {
                if (lanAddresses[key] <= Time.time)
                {
                    lanAddresses.Remove(key);
                    changed = true;
                }
            }

            if (changed)
                UpdateMatchInfo();

            yield return new WaitForSeconds(EXPIRE_TIMEOUT);
        }
    }

    /// <summary>
    /// Updates the UI list with the available games.
    /// </summary>
    private void UpdateMatchInfo()
    {
        if (SceneManager.GetActiveScene().name == "JoingameScene")
            AvailableGamesList.HandleNewGamesList(lanAddresses.Keys.ToList());
    }

    /// <summary>
    /// If you have problems with multicast lock on android, this method can help you
    /// </summary>
    void MulticastLock()
    {
        using (AndroidJavaObject activity = new AndroidJavaClass("com.unity3d.player.UnityPlayer").GetStatic<AndroidJavaObject>("currentActivity"))
        {
            using (var wifiManager = activity.Call<AndroidJavaObject>("getSystemService", "wifi"))
            {
                multicastLock = wifiManager.Call<AndroidJavaObject>("createMulticastLock", "lock");
                multicastLock.Call("acquire");
                Debug.Log("multicast lock acquired");
            }
        }
    }
}
