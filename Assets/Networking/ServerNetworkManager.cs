using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;

namespace UdeS.Promoscience.Network
{
    public class ServerUtils
    {
        public const float ServerNetworkManagerDelay = 1f;
        public const float PairingServerDelay = 1.5f;
    }


    public class ServerNetworkManager : NetworkManager
    {
        private static ServerNetworkManager _instance = null;


        public static ServerNetworkManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<ServerNetworkManager>();
                }

                return _instance;
            }
        }


        [SerializeField]
        ScriptableServerPlayerInformation serverPlayerInformation;

        const float informationSavingFrequencie = 10.0f;

        Action<String> playerDisconnectfromServerEvent;

        [SerializeField]
        private float timer = 0;

        [SerializeField]
        private bool isServerStarted = false;


        public void Persist()
        {
            if (_instance != null)
            {
                DestroyImmediate(gameObject);
                return;
            }

            transform.SetParent(null);
            _instance = Instance;
            DontDestroyOnLoad(gameObject);
        }

        public void Awake()
        {
            Persist();
        }

        private void Update()
        {
            // Return if destroyed
            if (gameObject == null)
                return;

            if (!isServerStarted)
            {
                isServerStarted = true;
                Server.Instance.LoadGameInformationFromDatabase();
                serverPlayerInformation.LoadPlayerInformationFromDatabase();
                playerDisconnectfromServerEvent += serverPlayerInformation.OnPlayerDisconnect;
                StartServer();
            }
            else if(isServerStarted)
            {
                timer += Time.deltaTime;

                if (timer >= informationSavingFrequencie)
                {
                    timer = 0;

                    serverPlayerInformation.SavePlayerInformationToDatabase();
                }
            }
        }

        public override void OnServerAddPlayer(NetworkConnection conn, short playerControllerId)
        {
            GameObject player = (GameObject)Instantiate(playerPrefab, Vector3.zero, Quaternion.identity);
            Player p = player.GetComponent<Player>();
            PlayerList.instance.AddPlayer(p);
            NetworkServer.AddPlayerForConnection(conn, player, playerControllerId);
        }

        public override void OnServerDisconnect(NetworkConnection conn)
        {
            if (playerDisconnectfromServerEvent != null)
            {
                Player player = PlayerList.instance.GetPlayerWithConnection(conn);

                if (player != null)
                {
                    playerDisconnectfromServerEvent(player.deviceUniqueIdentifier);
                }
            }
            base.OnServerDisconnect(conn);
        }

        public void CloseServer()
        {
            StopServer();

            Server.Instance.ClearGameInformation();
            serverPlayerInformation.ClearPlayerInformation();

            //Application.Quit();
        }
    }
}