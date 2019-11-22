using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Controls;
using Cirrus.Extensions;

namespace UdeS.Promoscience.Network
{
    public class Player : NetworkBehaviour
    {
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
                if(Client.Instance != null)
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
        GameActionManagerAsset gameAction;


        [SerializeField]
        DeviceTypeManagerAsset deviceType;

        [SerializeField]
        private AvatarControllerAsset controls;

        [SerializeField]
        GameRoundManagerAsset gameRound;

        //[SerializeField]
        //Labyrinths.ScriptableLabyrinth labyrinthData;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;


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
            if (Client.Instance == null)
                return;

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
            string labyrinth,
            Algorithms.Id algo,
            bool isTutorial)
        {
            controls.RecordedSteps.Value = new int[0];

            Client.Instance.LabyrinthData = JsonUtility.FromJson<Labyrinths.Data>(labyrinth);

            Labyrinths.Data data = JsonUtility.FromJson<Labyrinths.Data>(labyrinth);

            Client.Instance.Labyrinth = 
                Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(Client.Instance.LabyrinthData)
                .Create(Client.Instance.LabyrinthData);

            Client.Instance.Algorithm = Algorithms.Resources.Instance.GetAlgorithm(algo);

            gameRound.IsRoundCompleted.Value = false;

            gameRound.Round.Value = Client.Instance.LabyrinthData.Id;

            Client.Instance.State = isTutorial ? ClientGameState.PlayingTutorial : ClientGameState.Playing;
        }

        [TargetRpc]
        public void TargetSetGameWithSteps(
            NetworkConnection target, 
            int[] steps, 
            string labyrinth,
            Algorithms.Id algo,
            bool isTutorial)
        {
            controls.RecordedSteps.Value = steps;

            Client.Instance.LabyrinthData = JsonUtility.FromJson<Labyrinths.Data>(labyrinth);

            Client.Instance.Labyrinth = Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(Client.Instance.LabyrinthData)
                .Create(Client.Instance.LabyrinthData);

            Client.Instance.Algorithm = Algorithms.Resources.Instance.GetAlgorithm(algo);

            gameRound.IsRoundCompleted.Value = false;
            gameRound.Round.Value = Client.Instance.LabyrinthData.Id;

            Client.Instance.State = isTutorial ? ClientGameState.PlayingTutorial : ClientGameState.Playing;
        }

        [TargetRpc]
        public void TargetSetViewingLocalPlayback(NetworkConnection target, int labyrinthId, int[] steps, string[] stepValues)
        {
            gameRound.IsRoundCompleted.Value = true;
            gameRound.Round.Value = labyrinthId;
            //recordedSteps.Value = steps;
            Client.Instance.ActionValues = stepValues;
            Client.Instance.ActionSteps = steps;
            Client.Instance.State = ClientGameState.ViewingLocalReplay;
        }

        [TargetRpc]
        public void TargetSetViewingGlobalPlayback(NetworkConnection target, int labyrinthId, int[] steps, string[] stepValues)
        {
            gameRound.IsRoundCompleted.Value = true;
            gameRound.Round.Value = labyrinthId;

            // No steps required, player watch server screen
            Client.Instance.State = ClientGameState.ViewingGlobalReplay;
        }


        [TargetRpc]
        public void TargetSetRoundCompleted(NetworkConnection target, int labyrinthId, int[] steps)
        {
            gameRound.IsRoundCompleted.Value = true;
            gameRound.Round.Value = labyrinthId;
            controls.RecordedSteps.Value = steps;
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