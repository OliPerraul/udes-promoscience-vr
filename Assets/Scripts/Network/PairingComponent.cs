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
            player.playerStatusChangedEvent += StartPairingDevice;
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
        if (player.playerStatus == Constants.PAIRING)
        {
            pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.sDeviceType);

            if(pairedId == null)
            {
                player.playerStatus = Constants.NO_ASSOCIATED_PAIR;
                player.TargetSetPlayerStatus(player.connectionToClient ,Constants.NO_ASSOCIATED_PAIR);
            }
            else
            {
                StartCoroutine(PairingDeviceCoroutine());
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
                otherPlayer = PlayerList.instance.GetPlayerWithId(i);//Could be optimised to only look for unpared player?

                if (pairedId == otherPlayer.deviceUniqueIdentifier)
                {
                    pairedIpAdress = otherPlayer.connectionToClient.address;
                    break;
                }
            }
            yield return new WaitForSeconds(1);
        }

        player.TargetSetPairedIpAdress(player.connectionToClient, pairedIpAdress);

        if(player.sDeviceType == Constants.DEVICE_TABLET)
        {
            ScriptableTeam scriptableTeam = teamList.GetScriptableTeam();
            player.teamName = scriptableTeam.teamName;
            player.teamColor = scriptableTeam.teamColor;
            player.TargetSetPlayerAlgorithmId(player.connectionToClient, (scriptableTeam.teamId % 3) + 1);
            otherPlayer.teamName = scriptableTeam.teamName;
            otherPlayer.teamColor = scriptableTeam.teamColor;
            otherPlayer.TargetSetPlayerAlgorithmId(otherPlayer.connectionToClient, (scriptableTeam.teamId % 3) + 1);
        }
    }
#endif
}
