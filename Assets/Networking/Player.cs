using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Network
{
    public class Player : NetworkBehaviour
    {
        [SerializeField]
        private Resources resources;

        public string deviceUniqueIdentifier = "";
        string deviceName = "";

   
        private void OnDestroy()
        {
            if (isServer)
            {
                PlayerList.instance.RemovePlayer(this);
            }

            if (isClient)
            {
                Client.Instance.clientStateChangedEvent -= SendCmdPlayerGameState;

                if (deviceType.Value == Promoscience.DeviceType.Headset)
                {
                    gameAction.valueChangedEvent -= SendCmdPlayerAction;
                }
                else if (deviceType.Value == Promoscience.DeviceType.Tablet)
                {
                    playerInformation.playerInformationChangedEvent -= SendCmdPlayerInformation;
                }
            }
        }

        #region Server

        ClientGameState serverPlayerGameState = 0;
        GameAction serverPlayerGameAction;
        string serverPlayerGameActionValue;
        string serverPlayerGameActionDateTimeString;

        [SerializeField]
        int serverCourseId = -1;

        [SerializeField]
        int serverTeamId = -1;

        public Promoscience.DeviceType serverDeviceType = Promoscience.DeviceType.NoType;
        public Promoscience.Algorithms.Id serverAlgorithm;

        public int serverLabyrinthId;

        public int ServerCourseId
        {
            get
            {
                return serverCourseId;
            }
            set
            {
                serverCourseId = value;
                OnCourseIdChanged();
            }
        }

        public string ServerDeviceName
        {
            get
            {
                return deviceName;
            }
            set
            {
                deviceName = value;
                OnDeviceNameChanged();
            }
        }

        public int ServerTeamId
        {
            get
            {
                return serverTeamId;
            }
            set
            {
                serverTeamId = value;
                OnTeamIdChanged();
            }
        }

        public ClientGameState ServerPlayerGameState
        {
            get
            {
                return serverPlayerGameState;
            }
            set
            {
                serverPlayerGameState = value;
                OnPlayerStatusChanged();
            }
        }

        public GameAction ServerPlayerGameAction
        {
            get
            {
                return serverPlayerGameAction;
            }
        }

        public String ServerPlayerGameActionValue
        {
            get
            {
                return serverPlayerGameActionValue;
            }
        }

        public String ServerPlayerGameActionDateTimeString
        {
            get
            {
                return serverPlayerGameActionDateTimeString;
            }
        }

        public void ServerSetPlayerGameAction(GameAction action, string dateTime, string value)
        {
            serverPlayerGameAction = action;
            serverPlayerGameActionDateTimeString = dateTime;
            serverPlayerGameActionValue = value;

            serverPlayerActionChangedEvent();
        }

        public Action serverCourseIdChangedEvent;
        public Action serverDeviceNameChangedEvent;
        //public Action serverTeamIdChangedEvent;
        public Action serverTeamIdChangedEvent;
        public Action serverPlayerStatusChangedEvent;
        public Action serverPlayerActionChangedEvent;

        void OnCourseIdChanged()
        {
            if (serverCourseIdChangedEvent != null)
            {
                serverCourseIdChangedEvent();
            }
        }

        void OnDeviceNameChanged()
        {
            if (serverDeviceNameChangedEvent != null)
            {
                serverDeviceNameChangedEvent();
            }
        }

        void OnTeamIdChanged()
        {
            if (serverTeamIdChangedEvent != null)
            {
                serverTeamIdChangedEvent();
            }
        }

        void OnPlayerStatusChanged()
        {
            if (serverPlayerStatusChangedEvent != null)
            {
                serverPlayerStatusChangedEvent();
            }
        }

        #endregion

        #region Client

        [SerializeField]
        ScriptableGameAction gameAction;


        [SerializeField]
        ScriptableDeviceType deviceType;

        [SerializeField]
        ScriptableInteger gameRound;

        //[SerializeField]
        //ScriptableClientGameState client;

        [SerializeField]
        ScriptableBoolean isRoundCompleted;

        //[SerializeField]
        //Labyrinths.ScriptableLabyrinth labyrinthData;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;

        [SerializeField]
        ScriptableIntegerArray recordedSteps;

        //[SerializeField]
        //private ScriptableClientGameData gameData;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            Client.Instance.clientStateChangedEvent += SendCmdPlayerGameState;

            ClientInitialize();
        }

        [Client]
        void ClientInitialize()
        {
            // TODO
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            deviceUniqueIdentifier = Application.dataPath;// SystemInfo.deviceUniqueIdentifier;
#else
            deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
#endif

            ServerDeviceName = SystemInfo.deviceModel;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (deviceType.Value == Promoscience.DeviceType.Tablet)
            {
                playerInformation.playerInformationChangedEvent += SendCmdPlayerInformation;
            }
            else if (deviceType.Value == Promoscience.DeviceType.Headset)
            {
                gameAction.valueChangedEvent += SendCmdPlayerAction;
            }

            CmdSetDeviceType(deviceType.Value);
            CmdSetDeviceName(ServerDeviceName);
            CmdSetUniqueIdentifier(deviceUniqueIdentifier);

            Client.Instance.State = ClientGameState.Connecting;
        }

        [Client]
        void SendCmdPlayerAction()
        {
            if (Client.Instance.State == ClientGameState.Playing ||
                Client.Instance.State == ClientGameState.PlayingTutorial)
            {
                CmdSetPlayerAction(gameAction.Action, gameAction.DateTimeString, gameAction.Value);
            }
        }

        [Client]
        void SendCmdPlayerGameState()
        {
            CmdSetPlayerGameState(Client.Instance.State);
        }

        [Client]
        void SendCmdPlayerInformation()
        {
            CmdSetPlayerInformation(playerInformation.PlayerTeamId);
        }

#endregion

#region Command

        [Command]
        void CmdSetDeviceType(Promoscience.DeviceType type)
        {
            serverDeviceType = type;
        }

        [Command]
        void CmdSetDeviceName(string dName)
        {
            ServerDeviceName = dName;
        }

        [Command]
        public void CmdSetUniqueIdentifier(string id)
        {
            deviceUniqueIdentifier = id;
        }

        [Command]
        public void CmdSetPlayerAction(GameAction action, String dateTime, string value)
        {
            ServerSetPlayerGameAction(action, dateTime, value);
        }

        [Command]
        public void CmdSetPlayerGameState(ClientGameState state)
        {
            ServerPlayerGameState = state;
        }

        [Command]
        public void CmdSetPlayerInformation(int teamId)
        {
            ServerTeamId = teamId;
        }

#endregion

#region TargetRpc

        [TargetRpc]
        public void TargetSetGameState(NetworkConnection target, ClientGameState state)
        {
            Client.Instance.State = state;
        }

        [TargetRpc]
        public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
        {
            pairedIpAdress.Value = ipAdress;

            if (Client.Instance.State == ClientGameState.Pairing)
            {
                Client.Instance.State = ClientGameState.Paired;
            }
        }

        [TargetRpc]
        public void TargetSetGame(
            NetworkConnection target, 
            int[] data, 
            int sizeX, 
            int sizeY, 
            int labyrinthId, 
            Algorithms.Id algo,
            bool isTutorial)
        {
            recordedSteps.Value = new int[0];

            Client.Instance.LabyrinthData = new Labyrinths.Data(
                labyrinthId,
                data,
                sizeX,
                sizeY
                );

            Client.Instance.Labyrinth = 
                resources.Labyrinths
                .GetLabyrinth(Client.Instance.LabyrinthData)
                .Create(Client.Instance.LabyrinthData);

            Client.Instance.Algorithm = resources.Algorithms.CreateAlgorithm(algo);//, client.LabyrinthData);

            isRoundCompleted.Value = false;

            gameRound.Value = labyrinthId;

            Client.Instance.State = isTutorial ? ClientGameState.PlayingTutorial : ClientGameState.Playing;


        }

        [TargetRpc]
        public void TargetSetGameWithSteps(
            NetworkConnection target, 
            int[] steps, 
            int[] data, 
            int sizeX, 
            int sizeY, 
            int labyrinthId, 
            Algorithms.Id algo,
            bool isTutorial)
        {
            recordedSteps.Value = steps;

            Client.Instance.LabyrinthData = new Labyrinths.Data(
                labyrinthId,
                data,
                sizeX,
                sizeY);


            Client.Instance.Labyrinth = resources.Labyrinths
                .GetLabyrinth(Client.Instance.LabyrinthData)
                .Create(Client.Instance.LabyrinthData);

            Client.Instance.Algorithm = resources.Algorithms.CreateAlgorithm(algo);/// client.LabyrinthData);

            isRoundCompleted.Value = false;
            gameRound.Value = labyrinthId;

            Client.Instance.State = isTutorial ? ClientGameState.PlayingTutorial : ClientGameState.Playing;
        }

        [TargetRpc]
        public void TargetSetViewingLocalPlayback(NetworkConnection target, int labyrinthId, int[] steps, string[] stepValues)
        {
            isRoundCompleted.Value = true;
            gameRound.Value = labyrinthId;
            //recordedSteps.Value = steps;
            Client.Instance.ActionValues = stepValues;
            Client.Instance.ActionSteps = steps;
            Client.Instance.State = ClientGameState.ViewingLocalReplay;
        }

        [TargetRpc]
        public void TargetSetViewingGlobalPlayback(NetworkConnection target, int labyrinthId, int[] steps, string[] stepValues)
        {
            isRoundCompleted.Value = true;
            gameRound.Value = labyrinthId;

            // No steps required, player watch server screen
            Client.Instance.State = ClientGameState.ViewingGlobalReplay;
        }


        [TargetRpc]
        public void TargetSetRoundCompleted(NetworkConnection target, int labyrinthId, int[] steps)
        {
            isRoundCompleted.Value = true;
            gameRound.Value = labyrinthId;
            recordedSteps.Value = steps;
            Client.Instance.State = ClientGameState.WaitingForNextRound;
        }

        [TargetRpc]
        public void TargetSetEndRoundOrTutorial(NetworkConnection target)
        {
            Client.Instance.Labyrinth.gameObject.Destroy();
            Client.Instance.Labyrinth = null;
        }

        [TargetRpc]
        public void TargetSetTeamInformation(NetworkConnection target, int teamId)
        {
            playerInformation.SetPlayerInformation(teamId);
        }
#endregion

    }
}