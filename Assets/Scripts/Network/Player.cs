using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

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
                gameState.valueChangedEvent -= SendCmdPlayerGameState;

                if (deviceType.Value == Utils.DeviceType.Headset)
                {
                    gameAction.valueChangedEvent -= SendCmdPlayerAction;
                }
                else if (deviceType.Value == Utils.DeviceType.Tablet)
                {
                    playerInformation.playerInformationChangedEvent -= SendCmdPlayerInformation;
                }
            }
        }

        #region Server

        ClientGameState serverPlayerGameState = 0;
        GameAction serverPlayerGameAction;
        string serverPlayerGameActionDateTimeString;

        int serverCourseId = -1;
        int serverTeamId = -1;
        int serverTeamInformationId = -1;

        [SerializeField]
        public UnityEngine.UI.Text DebugText;

        public Utils.DeviceType serverDeviceType = Utils.DeviceType.NoType;
        public Utils.Algorithm serverAlgorithm;

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

        public int ServerTeamInformationId
        {
            get
            {
                return serverTeamInformationId;
            }
            set
            {
                serverTeamInformationId = value;
                OnTeamInformationIdChanged();
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

        public String ServerPlayerGameActionDateTimeString
        {
            get
            {
                return serverPlayerGameActionDateTimeString;
            }
        }

        public void ServerSetPlayerGameAction(GameAction action, String dateTime)
        {
            serverPlayerGameAction = action;
            serverPlayerGameActionDateTimeString = dateTime;

            serverPlayerActionChangedEvent();
        }

        public Action serverCourseIdChangedEvent;
        public Action serverDeviceNameChangedEvent;
        public Action serverTeamIdChangedEvent;
        public Action serverTeamInformationIdChangedEvent;
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

        void OnTeamInformationIdChanged()
        {
            if (serverTeamInformationIdChangedEvent != null)
            {
                serverTeamInformationIdChangedEvent();
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
        ScriptableAlgorithm algorithm;

        [SerializeField]
        ScriptableDeviceType deviceType;

        [SerializeField]
        ScriptableInteger gameRound;

        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        ScriptableBoolean isRoundCompleted;

        [SerializeField]
        ScriptableLabyrinth labyrinthData;

        [SerializeField]
        ScriptableString pairedIpAdress;

        [SerializeField]
        ScriptablePlayerInformation playerInformation;

        [SerializeField]
        ScriptableIntegerArray recordedSteps;

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            gameState.valueChangedEvent += SendCmdPlayerGameState;

            ClientInitialize();
        }

        [Client]
        void ClientInitialize()
        {
            deviceUniqueIdentifier = SystemInfo.deviceUniqueIdentifier;
            ServerDeviceName = SystemInfo.deviceModel;

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (deviceType.Value == Utils.DeviceType.Tablet)
            {
                playerInformation.playerInformationChangedEvent += SendCmdPlayerInformation;
            }
            else if (deviceType.Value == Utils.DeviceType.Headset)
            {
                gameAction.valueChangedEvent += SendCmdPlayerAction;
            }

            CmdSetDeviceType(deviceType.Value);
            CmdSetDeviceName(ServerDeviceName);
            CmdSetUniqueIdentifier(deviceUniqueIdentifier);

            gameState.Value = ClientGameState.Connecting;
        }

        [Client]
        void SendCmdPlayerAction()
        {
            if (gameState.Value == ClientGameState.Playing ||
                gameState.Value == ClientGameState.PlayingTutorial)
            {
                CmdSetPlayerAction(gameAction.Action, gameAction.DateTimeString);
            }
        }

        [Client]
        void SendCmdPlayerGameState()
        {
            CmdSetPlayerGameState(gameState.Value);
        }

        [Client]
        void SendCmdPlayerInformation()
        {
            CmdSetPlayerInformation(playerInformation.PlayerTeamInformationId);
        }

        #endregion

        #region Command

        [Command]
        void CmdSetDeviceType(Utils.DeviceType type)
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
        public void CmdSetPlayerAction(GameAction action, String dateTime)
        {
            ServerSetPlayerGameAction(action, dateTime);
        }

        [Command]
        public void CmdSetPlayerGameState(ClientGameState state)
        {
            ServerPlayerGameState = state;
        }

        [Command]
        public void CmdSetPlayerInformation(int teamInformationId)
        {
            ServerTeamInformationId = teamInformationId;
        }

        #endregion

        #region TargetRpc

        [TargetRpc]
        public void TargetSetGameState(NetworkConnection target, ClientGameState state)
        {
            gameState.Value = state;
        }

        [TargetRpc]
        public void TargetSetPairedIpAdress(NetworkConnection target, string ipAdress)
        {
            pairedIpAdress.Value = ipAdress;

            if (gameState.Value == ClientGameState.Pairing)
            {
                gameState.Value = ClientGameState.Paired;
            }
        }

        [TargetRpc]
        public void TargetSetGame(NetworkConnection target, int[] data, int sizeX, int sizeY, int labyrinthId, Utils.Algorithm algo)
        {
            recordedSteps.Value = new int[0];

            labyrinthData.SetLabyrithData(data, sizeX, sizeY, labyrinthId);

            isRoundCompleted.Value = false;
            gameRound.Value = labyrinthId;

            if (deviceType.Value == Utils.DeviceType.Headset)
            {
                algorithm.Value = algo;
            }

            if (algo == Utils.Algorithm.Tutorial)
            {
                gameState.Value = ClientGameState.TutorialLabyrinthReady;
            }
            else
            {
                gameState.Value = ClientGameState.LabyrithReady;
            }
        }

        [TargetRpc]
        public void TargetSetGameWithSteps(NetworkConnection target, int[] steps, int[] data, int sizeX, int sizeY, int labyrinthId, Utils.Algorithm algo)
        {
            recordedSteps.Value = steps;

            labyrinthData.SetLabyrithData(data, sizeX, sizeY, labyrinthId);
            algorithm.Value = algo;

            isRoundCompleted.Value = false;
            gameRound.Value = labyrinthId;

            if (algorithm.Value == Utils.Algorithm.Tutorial)
            {
                gameState.Value = ClientGameState.TutorialLabyrinthReady;
            }
            else
            {
                gameState.Value = ClientGameState.LabyrithReady;
            }
        }

        [TargetRpc]
        public void TargetSetViewingPlayback(NetworkConnection target, int labyrinthId, int[] steps)
        {
            isRoundCompleted.Value = true;
            gameRound.Value = labyrinthId;
            recordedSteps.Value = steps; // Use for playback
            gameState.Value = ClientGameState.ViewingPlayback;
        }


        [TargetRpc]
        public void TargetSetRoundCompleted(NetworkConnection target, int labyrinthId, int[] steps)
        {
            isRoundCompleted.Value = true;
            gameRound.Value = labyrinthId;
            recordedSteps.Value = steps; 
            gameState.Value = ClientGameState.WaitingForNextRound;
        }

        [TargetRpc]
        public void TargetSetTeamInformation(NetworkConnection target, int teamInformationId)
        {
            playerInformation.SetPlayerInformation(teamInformationId);
        }
        #endregion

    }
}