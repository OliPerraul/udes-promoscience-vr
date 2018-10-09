using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStatus : MonoBehaviour
{
    public void StartGame()
    {
        for(int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if(player.playerStatus == Constants.PLAYING_TUTORIAL|| player.playerStatus == Constants.WAITING_FOR_NEXT_ROUND)
            {
                player.TargetSetPlayerStatus(player.connectionToClient, Constants.READY);
            }
        }
    }
}
