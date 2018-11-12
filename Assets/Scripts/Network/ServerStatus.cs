﻿using System.Collections;
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

        int[] data = labyrinthData.GetLabyrithDataWithId(((gameRound - 1) % 3) + 1);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if(player.ServerPlayerGameState == GameState.PlayingTutorial || player.ServerPlayerGameState == GameState.Playing || player.ServerPlayerGameState == GameState.WaitingForNextRound)
            {
                player.serverAlgorithm = (Algorithm) (((int) player.serverAlgorithm) % 3) + 1;
                player.TargetSetPlayerAlgorithm(player.connectionToClient, player.serverAlgorithm);
                player.serverLabyrinthId = gameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                player.serverCourseId = SQLiteUtilities.GetNextCourseID();
#endif
                player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound);
            }
        }
    }
}
