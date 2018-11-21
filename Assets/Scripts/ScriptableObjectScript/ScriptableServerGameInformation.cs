using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/ServerGameState", order = 1)]
public class ScriptableServerGameInformation : ScriptableObject
{
    [SerializeField]
    ServerGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    public Action gameStateChangedEvent;

    const int tutorialLabyrinthId = 4;

    int gameRound = 0;

    public ServerGameState GameState
    {
        get
        {
            return gameState;
        }
        set
        {
            gameState = value;
            OnGameStateValueChanged();
        }
    }

    public void OnGameStateValueChanged()
    {
        if (gameStateChangedEvent != null)
        {
            gameStateChangedEvent();
        }
    }


    public void StartTutorial()
    {
        gameState = ServerGameState.Tutorial;

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

    public void StartTutorial(Player player)
    {
        int[] data = labyrinthData.GetLabyrithDataWithId(tutorialLabyrinthId);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        if (player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
        {
            Algorithm algorithm = Algorithm.Tutorial;

            player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, tutorialLabyrinthId, algorithm);
        }
    }

    public void StartNextGameRound()
    {
        gameState = ServerGameState.GameRound;

        gameRound++;

        int[] data = labyrinthData.GetLabyrithDataWithId(((gameRound - 1) % 3) + 1);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if (player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
            {
                Algorithm algorithm = (Algorithm)(((int)player.serverAlgorithm) % 3) + 1;
                player.serverAlgorithm = algorithm;
                player.serverLabyrinthId = gameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif
                player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound, algorithm);
            }
        }
    }

    public void StartGameRound(Player player)
    {
        int[] data = labyrinthData.GetLabyrithDataWithId(((gameRound - 1) % 3) + 1);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        if (player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
        {
            Algorithm algorithm = (Algorithm)(((int)player.serverAlgorithm) % 3) + 1;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = gameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif
            player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound, algorithm);
        }
    }

    public void EndRoundOrTutorial()
    {
        gameState = ServerGameState.Intermission;

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

