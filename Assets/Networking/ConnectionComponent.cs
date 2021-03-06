﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Network
{
    public class ConnectionComponent : NetworkBehaviour
    {
        [SerializeField]
        Player player;

        //[SerializeField]
        //private Teams.Resources teamList;

        //[SerializeField]
        //private ScriptableServerGameInformation serverGameInformation;

        [SerializeField]
        ScriptableServerPlayerInformation serverPlayerInformation;

        string pairedId = null;

        void Start()
        {
            if (isServer)
            {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

                player.serverPlayerStatusChangedEvent += OnPlayerStatusChanged;
#endif
            }
            else
            {
                Destroy(this);
            }
        }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        private void OnPlayerStatusChanged()
        {
            if (player.ServerPlayerGameState == ClientGameState.Connecting)
            {
                serverPlayerInformation.AddPlayerOrReconnect(player);
            }
            else if (player.ServerPlayerGameState == ClientGameState.Pairing)
            {
                pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.serverDeviceType);

                if (pairedId == null)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.NoAssociatedPair);
                }
                else
                {
                    // BUG FIX??
                    // Team Id not assigned to tablet on reconnect
                    if (player.serverDeviceType == Promoscience.DeviceType.Headset)
                    {
                        InitializeHeadsetInformation();
                    }
                    else
                    {
                        StartCoroutine(PairingDeviceCoroutine());
                    }
                }
            }
            else if (player.ServerPlayerGameState == ClientGameState.WaitingReplay) 
            {
                // TODO
                // Steps to recover
                Queue<int> steps;
                Queue<string> stepValues; //jsons
                SQLiteUtilities.GetPlayerStepsForCourse(player.ServerCourseId, out steps, out stepValues);

                // Use the steps for playback
                player.TargetSetViewingLocalPlayback(
                    player.connectionToClient,
                    GameManager.Instance.CurrentGame.CurrentLevel.Labyrinth.Id, 
                    steps.ToArray(), 
                    stepValues.ToArray());
            }
            else if (player.serverDeviceType == Promoscience.DeviceType.Headset && player.ServerPlayerGameState == ClientGameState.Reconnecting)
            {
                pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.serverDeviceType);

                if (pairedId == null)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ReconnectingNoAssociatedPair);
                }
                else
                {
                    if (Server.Instance.State.Value == ServerState.Level && player.ServerCourseId != -1)
                    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                        int courseLabyrinthId = SQLiteUtilities.GetPlayerCourseLabyrinthId(player.ServerCourseId);
#endif
                        if (courseLabyrinthId == GameManager.Instance.CurrentGame.CurrentLevel.Labyrinth.Id)
                        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

                            Queue<int> steps;

                            steps = SQLiteUtilities.GetPlayerStepsForCourse(player.ServerCourseId);

                            if (SQLiteUtilities.HasPlayerAlreadyCompletedTheRound(player.ServerCourseId))
                            {
                                // Use the steps for playback
                                player.TargetSetPairedIpAdress(
                                    player.connectionToClient, 
                                    "");

                                player.TargetSetLevelCompleted(
                                    player.connectionToClient,
                                    GameManager.Instance.CurrentGame.CurrentLevel.Labyrinth.Id,
                                    steps.ToArray());
                            }
                            else
                            {
                                if (steps.Count > 0)
                                {
                                    // Connection drop, used the steps to resume where you were
                                    GameManager.Instance.CurrentGame.JoinGameLevelWithSteps(player, steps.ToArray());
                                    player.TargetSetPairedIpAdress(player.connectionToClient, "");
                                }
                                else
                                {
                                    player.TargetSetPairedIpAdress(player.connectionToClient, "");
                                    player.TargetSetGameState(player.connectionToClient, ClientGameState.Ready);
                                }
                            }
#endif
                        }
                        else
                        {
                            player.TargetSetPairedIpAdress(player.connectionToClient, "");
                            player.TargetSetGameState(player.connectionToClient, ClientGameState.Ready);
                        }
                    }
                    else
                    {
                        player.TargetSetPairedIpAdress(player.connectionToClient, "");
                        player.TargetSetGameState(player.connectionToClient, ClientGameState.Ready);
                    }
                }
            }
            else if (player.ServerPlayerGameState == ClientGameState.Ready)
            {
                if (
                    Server.Instance.State.Value == ServerState.Level ||
                    Server.Instance.State.Value == ServerState.Quickplay)
                {
                    GameManager.Instance.CurrentGame.JoinGameLevel(player);
                }
                else if (Server.Instance.State.Value == ServerState.Intermission)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextLevel);
                }
            }
        }

        void InitializeHeadsetInformation()
        {
            Teams.TeamResource scriptableTeam = Teams.Resources.Instance.GetUnusedScriptableTeam();

            player.serverAlgorithm = (Promoscience.Algorithms.Id)(scriptableTeam.TeamId % 3) + 1;
            player.ServerTeamId = scriptableTeam.TeamId;
            player.TargetSetTeamInformation(player.connectionToClient, scriptableTeam.TeamId);
            player.TargetSetPairedIpAdress(player.connectionToClient, "");
            player.TargetSetGameState(player.connectionToClient, ClientGameState.Ready);
        }

        IEnumerator PairingDeviceCoroutine()
        {
            string pairedIpAdress = null;
            Player otherPlayer = null;
            while (pairedIpAdress == null)
            {
                for (int i = 0; i < PlayerList.instance.list.Count; i++)
                {
                    otherPlayer = PlayerList.instance.GetPlayerWithId(i);

                    //FIX: No team assigned if not in pairing state ?? wtf
                    // obv not the right place to put this
                    // Work around/ fix assign server team right here;

                    if (pairedId == otherPlayer.deviceUniqueIdentifier)
                    {
                        player.ServerTeamId = otherPlayer.ServerTeamId;
                        player.ServerCourseId = otherPlayer.ServerCourseId;
                        player.ServerLevelNumber = otherPlayer.ServerLevelNumber;
                        player.serverLabyrinthId = otherPlayer.serverLabyrinthId;
                        //player.ser

                        pairedIpAdress = otherPlayer.connectionToClient.address;
                        break;
                    }
                }
                yield return new WaitForSeconds(1);
            }

            player.TargetSetPairedIpAdress(player.connectionToClient, pairedIpAdress);
        }
#endif
    }
}