using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStatus : MonoBehaviour
{
    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    int gameRound = 0;

    public void StartGame()
    {
        gameRound++;

        int[] data = labyrinthData.GetLabyrithDataWitId(gameRound);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if(player.playerStatus == Constants.PLAYING_TUTORIAL|| player.playerStatus == Constants.WAITING_FOR_NEXT_ROUND)
            {
                player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound);
            }
        }
    }
}
