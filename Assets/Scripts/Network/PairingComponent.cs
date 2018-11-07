using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PairingComponent : NetworkBehaviour
{
    [SerializeField]
    Player player;

    [SerializeField]
    ScriptableTeamList teamList;

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
        if (player.ServerPlayerGameState == GameState.Pairing)
        {
            pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.serverDeviceType);

            if(pairedId == null)
            {
                player.ServerPlayerGameState = GameState.NoAssociatedPair;
                player.TargetSetGameState(player.connectionToClient , GameState.NoAssociatedPair);
            }
            else
            {
                if (player.serverDeviceType == DeviceType.Headset)
                {
                    StartCoroutine(PairingDeviceCoroutine());
                }
            }
        }
    }

    IEnumerator PairingDeviceCoroutine()
    {
        string pairedIpAdress = null;
        Player otherPlayer = null;
        while (pairedIpAdress == null)
        {
            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                otherPlayer = PlayerList.instance.GetPlayerWithId(i);//Could be optimised to only look for unpared player? et pourrait être selement la tablette qui cherche le paire (vérifier reconnection ?)

                if (pairedId == otherPlayer.deviceUniqueIdentifier)
                {
                    pairedIpAdress = otherPlayer.connectionToClient.address;
                    break;
                }
            }
            yield return new WaitForSeconds(1);
        }

        ScriptableTeam scriptableTeam = teamList.GetScriptableTeam();
        int teamId = SQLiteUtilities.GetNextTeamID();

        player.ServerTeamName = scriptableTeam.teamName;
        player.ServerTeamColor = scriptableTeam.teamColor;
        player.serverAlgorithm = (Algorithm)(scriptableTeam.teamId % 3) + 1;
        player.serverTeamId = teamId;
        otherPlayer.ServerTeamName = scriptableTeam.teamName;
        otherPlayer.ServerTeamColor = scriptableTeam.teamColor;
        otherPlayer.serverAlgorithm = player.serverAlgorithm;
        otherPlayer.serverTeamId = teamId;

        player.TargetSetPairedIpAdress(player.connectionToClient, pairedIpAdress);
        otherPlayer.TargetSetPairedIpAdress(otherPlayer.connectionToClient, player.connectionToClient.address);
    }
#endif
}
