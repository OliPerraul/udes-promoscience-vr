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

        int[] data = labyrinthData.GetLabyrithDataWithId(gameRound);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if(player.sPlayerStatus == Constants.PLAYING_TUTORIAL|| player.sPlayerStatus == Constants.WAITING_FOR_NEXT_ROUND)
            {
                player.sAlgorithmId = ((player.sAlgorithmId + 1) % 3) + 1;
                player.TargetSetPlayerAlgorithmId(player.connectionToClient, player.sAlgorithmId);
                player.sLabyrinthId = gameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                player.sCourseId = SQLiteUtilities.GetNextCourseID();
#endif
                
                player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound);
            }
        }
    }
}
