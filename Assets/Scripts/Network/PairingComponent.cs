using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PairingComponent : NetworkBehaviour
{
    [SerializeField]
    Player player;

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
            pairedId = SQLiteUtilities.GetPairing(player.deviceUniqueIdentifier, player.deviceType);

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

        while (pairedIpAdress == null)
        {
            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player p = PlayerList.instance.GetPlayerWithId(i);//Could be optimised to only look for unpared player?

                if (pairedId == p.deviceUniqueIdentifier)
                {
                    pairedIpAdress = p.connectionToClient.address;
                    break;
                }
            }
            yield return new WaitForSeconds(1);
        }

        player.TargetSetPairedIpAdress(player.connectionToClient, pairedIpAdress);
    }
#endif
}
