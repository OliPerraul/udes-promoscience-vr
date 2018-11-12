using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ServerStatus : MonoBehaviour
{
    [SerializeField]
    ScriptableServerGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    int gameRound = 0;

    const int tutorialLabyrinthId = 4;

    public void StartTutorial()
    {
        gameState.Value = ServerGameState.Tutorial;

        int[] data = labyrinthData.GetLabyrithDataWithId(tutorialLabyrinthId);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if (player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
            {
                Algorithm algorithm = Algorithm.Tutorial;

                player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, tutorialLabyrinthId, algorithm);
            }
        }
    }

    public void StartNextGameRound()
    {
        gameState.Value = ServerGameState.GameRound;

        gameRound++;

        int[] data = labyrinthData.GetLabyrithDataWithId(((gameRound - 1) % 3) + 1);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if(player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
            {
                Algorithm algorithm = (Algorithm) (((int) player.serverAlgorithm) % 3) + 1;
                player.serverAlgorithm = algorithm;
                player.serverLabyrinthId = gameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                player.serverCourseId = SQLiteUtilities.GetNextCourseID();
#endif
                player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound, algorithm);
            }
        }
    }

    public void EndRoundOrTutorial()
    {
        gameState.Value = ServerGameState.Intermission;

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if (player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing)
            {
                player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextRound);
            }
        }
    }
}
