using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Network
{
    public class ConnectionComponent : NetworkBehaviour
    {
        [SerializeField]
        Player player;

        [SerializeField]
        ScriptableTeamList teamList;

        [SerializeField]
        ScriptableServerGameInformation serverGameInformation;

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
                    if (player.serverDeviceType == Utils.DeviceType.Headset)
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
                player.TargetSetViewingLocalPlayback(player.connectionToClient, serverGameInformation.GameRound, steps.ToArray(), stepValues.ToArray());                

            }
            else if (player.serverDeviceType == Utils.DeviceType.Headset && player.ServerPlayerGameState == ClientGameState.Reconnecting)
            {
                pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.serverDeviceType);

                if (pairedId == null)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ReconnectingNoAssociatedPair);
                }
                else
                {
                    if (serverGameInformation.GameState == ServerGameState.GameRound && player.ServerCourseId != -1)
                    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                        int courseLabyrinthId = SQLiteUtilities.GetPlayerCourseLabyrinthId(player.ServerCourseId);
#endif
                        if (courseLabyrinthId == serverGameInformation.GameRound)
                        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN

                            Queue<int> steps;

                            steps = SQLiteUtilities.GetPlayerStepsForCourse(player.ServerCourseId);

                            if (SQLiteUtilities.HasPlayerAlreadyCompletedTheRound(player.ServerCourseId))
                            {
                                // Use the steps for playback
                                player.TargetSetPairedIpAdress(player.connectionToClient, "");
                                player.TargetSetRoundCompleted(player.connectionToClient, serverGameInformation.GameRound, steps.ToArray());
                            }
                            else
                            {
                                if (steps.Count > 0)
                                {
                                    // Connection drop, used the steps to resume where you were
                                    serverGameInformation.StartGameRoundWithSteps(player, steps.ToArray());
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
                if (serverGameInformation.GameState == ServerGameState.Tutorial)
                {
                    serverGameInformation.StartTutorial(player);
                }
                else if (serverGameInformation.GameState == ServerGameState.GameRound)
                {
                    serverGameInformation.StartGameRound(player);
                }
                else if (serverGameInformation.GameState == ServerGameState.Intermission)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextRound);
                }
            }
        }

        void InitializeHeadsetInformation()
        {
            ScriptableTeam scriptableTeam = teamList.GetUnusedScriptableTeam();

            player.serverAlgorithm = (Utils.Algorithm)(scriptableTeam.TeamId % 3) + 1;
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

                    if (pairedId == otherPlayer.deviceUniqueIdentifier)
                    {
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