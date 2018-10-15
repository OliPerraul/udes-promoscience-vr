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
            player.sPlayerStatusChangedEvent += StartPairingDevice;
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
        if (player.sPlayerStatus == Constants.PAIRING)
        {
            pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.sDeviceType);

            if(pairedId == null)
            {
                player.sPlayerStatus = Constants.NO_ASSOCIATED_PAIR;
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
            int teamId = SQLiteUtilities.GetNextTeamID();

            player.sTeamName = scriptableTeam.teamName;
            player.sTeamColor = scriptableTeam.teamColor;
            player.sAlgorithmId = (scriptableTeam.teamId % 3) + 1;
            player.sTeamId = teamId;
            otherPlayer.sTeamName = scriptableTeam.teamName;
            otherPlayer.sTeamColor = scriptableTeam.teamColor;
            otherPlayer.sAlgorithmId = player.sAlgorithmId;
            otherPlayer.sTeamId = teamId;
        }
    }
#endif
}
