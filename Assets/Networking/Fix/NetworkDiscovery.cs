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

public class NetworkDiscoveryFix : NetworkDiscovery
{
    private NetworkDiscoveryFix instance;

    public NetworkDiscoveryFix Instance
    {
        get
        {
            if (instance == null)
                instance = FindObjectOfType<NetworkDiscoveryFix>();

            return instance;
        }
    }


    private const float EXPIRE_TIMEOUT = 2f;

    private bool isBroadcasting = false;
    private bool isListening = false;

    private AndroidJavaObject multicastLock;

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
            
            if (NetworkTransport.IsStarted && NetworkTransport.IsBroadcastDiscoveryRunning())
                NetworkTransport.StopBroadcastDiscovery();
            NetworkTransport.Shutdown();          
            NetworkTransport.Init();

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
            if (NetworkTransport.IsStarted && NetworkTransport.IsBroadcastDiscoveryRunning())
                NetworkTransport.StopBroadcastDiscovery();
            
            NetworkTransport.Shutdown();
            NetworkTransport.Init();

            Initialize();
            StartAsClient();

            isListening = true;
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

            if (NetworkTransport.IsStarted && NetworkTransport.IsBroadcastDiscoveryRunning())
                NetworkTransport.StopBroadcastDiscovery();
            NetworkTransport.Shutdown();

        }
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
