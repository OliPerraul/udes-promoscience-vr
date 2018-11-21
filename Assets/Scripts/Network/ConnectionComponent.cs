using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

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

    void Start ()
    {
        if (isServer)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            player.serverPlayerStatusChangedEvent += StartPairingDevice;
#endif
        }
        else
        {
            Destroy(this);
        }
    }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

    void StartPairingDevice()
    {
        if (player.ServerPlayerGameState == ClientGameState.Connecting)
        {
           serverPlayerInformation.AddPlayerOrReconnect(PlayerList.instance.GetPlayerId(player), player.deviceUniqueIdentifier);
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
                if (player.serverDeviceType == DeviceType.Headset)
                {
                    InitializeHeadsetInformation();
                }
                else
                {
                    StartCoroutine(PairingDeviceCoroutine());
                }
            }
        }
        else if (player.serverDeviceType == DeviceType.Headset && player.ServerPlayerGameState == ClientGameState.Reconnecting)
        {
            pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.serverDeviceType);

            if (pairedId == null)
            {
                player.TargetSetGameState(player.connectionToClient, ClientGameState.ReconnectingNoAssociatedPair);
            }
            else
            {
                /*
                if (serverGameInformation.GameState == ServerGameState.GameRound)
                {
                    //BD get steps for current gameround
                    //And labyrinth

                    //StartWithStepFunction?
                }
                else
                {
                    player.TargetSetPairedIpAdress(player.connectionToClient, "");
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.Ready);//Set the other game state tat start the MessageServer?
                }
                */

                //Temp
                player.TargetSetPairedIpAdress(player.connectionToClient, "");
            }
        }
        else if (player.ServerPlayerGameState == ClientGameState.Ready)
        {
            if(serverGameInformation.GameState == ServerGameState.Tutorial)
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
        int teamId = SQLiteUtilities.GetNextTeamID();

        player.serverAlgorithm = (Algorithm)(scriptableTeam.TeamId % 3) + 1;
        player.ServerTeamId = teamId;
        player.ServerTeamInformationId = scriptableTeam.TeamId;

        player.TargetSetTeamInformation(player.connectionToClient, scriptableTeam.TeamId);
        player.TargetSetPairedIpAdress(player.connectionToClient, "");//Change this for change game state?
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
