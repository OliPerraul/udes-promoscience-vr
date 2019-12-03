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
                    Client.Instance.State.OnValueChangedHandler -= SendCmdPlayerGameState;

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
        GameAsset gameManager;

        //[SerializeField]
        //Labyrinths.ScriptableLabyrinth labyrinthData;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;


        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            Client.Instance.State.OnValueChangedHandler += SendCmdPlayerGameState;

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

            Client.Instance.State.Value = ClientGameState.Connecting;
        }

        [Client]
        void SendCmdPlayerAction()
        {
            if (Client.Instance == null)
                return;

            if (Client.Instance.State.Value == ClientGameState.Playing ||
                Client.Instance.State.Value == ClientGameState.PlayingTutorial)
            {
                CmdSetPlayerAction(gameAction.Action, gameAction.DateTimeString, gameAction.Value);
            }
        }

        [Client]
        void SendCmdPlayerGameState(ClientGameState state)
        {
            CmdSetPlayerGameState(state);
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
            Client.Instance.State.Value = state;
        }

        [TargetRpc]
        public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
        {
            pairedIpAdress.Value = ipAdress;

            if (Client.Instance.State.Value == ClientGameState.Pairing)
            {
                Client.Instance.State.Value = ClientGameState.Paired;
            }
        }

        [TargetRpc]
        public void TargetSetGame(
            NetworkConnection target, 
            string labyrinth,
            Algorithms.Id algo,
            int round)
        {
            controls.RecordedSteps.Value = new int[0];

            Client.Instance.LabyrinthData.Value = JsonUtility.FromJson<Labyrinths.Data>(labyrinth);

            Labyrinths.Data data = JsonUtility.FromJson<Labyrinths.Data>(labyrinth);

            Client.Instance.Labyrinth.Value = 
                Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(Client.Instance.LabyrinthData.Value)
                .Create(Client.Instance.LabyrinthData.Value);

            Client.Instance.Algorithm.Set(Algorithms.Resources.Instance.GetAlgorithm(algo));

            gameManager.IsRoundCompleted.Value = false;

            gameManager.Round.Value = round;

            Client.Instance.State.Set(ClientGameState.Playing);
        }

        [TargetRpc]
        public void TargetSetGameWithSteps(
            NetworkConnection target, 
            int[] steps, 
            string labyrinth,
            Algorithms.Id algo,
            int round,
            bool isTutorial)
        {
            controls.RecordedSteps.Value = steps;

            Client.Instance.LabyrinthData.Value = JsonUtility.FromJson<Labyrinths.Data>(labyrinth);

            Client.Instance.Labyrinth.Value = Labyrinths.Resources.Instance
                .GetLabyrinthTemplate(Client.Instance.LabyrinthData.Value)
                .Create(Client.Instance.LabyrinthData.Value);

            Client.Instance.Algorithm.Set(Algorithms.Resources.Instance.GetAlgorithm(algo));

            gameManager.IsRoundCompleted.Value = false;

            gameManager.Round.Value = round;

            Client.Instance.State.Value = isTutorial ? ClientGameState.PlayingTutorial : ClientGameState.Playing;
        }

        [TargetRpc]
        public void TargetSetViewingLocalPlayback(NetworkConnection target, int labyrinthId, int[] steps, string[] stepValues)
        {
            gameManager.IsRoundCompleted.Value = true;
            gameManager.Round.Value = labyrinthId;
            //recordedSteps.Value = steps;
            Client.Instance.ActionValues = stepValues;
            Client.Instance.ActionSteps = steps;
            Client.Instance.State.Value = ClientGameState.ViewingLocalReplay;
        }

        [TargetRpc]
        public void TargetSetViewingGlobalPlayback(NetworkConnection target, int labyrinthId, int[] steps, string[] stepValues)
        {
            gameManager.IsRoundCompleted.Value = true;
            gameManager.Round.Value = labyrinthId;

            // No steps required, player watch server screen
            Client.Instance.State.Value = ClientGameState.ViewingGlobalReplay;
        }

        [TargetRpc]
        public void TargetSetRoundCompleted(NetworkConnection target, int labyrinthId, int[] steps)
        {
            gameManager.IsRoundCompleted.Value = true;
            gameManager.Round.Value = labyrinthId;
            controls.RecordedSteps.Value = steps;
            Client.Instance.State.Value = ClientGameState.WaitingForNextRound;
        }

        [TargetRpc]
        public void TargetSetEndRoundOrTutorial(NetworkConnection target)
        {
            Client.Instance.Labyrinth.Value.gameObject.Destroy();
            Client.Instance.Labyrinth.Value = null;
        }

        [TargetRpc]
        public void TargetSetTeamInformation(NetworkConnection target, int teamId)
        {
            playerInformation.SetPlayerInformation(teamId);
        }
#endregion

    }
}