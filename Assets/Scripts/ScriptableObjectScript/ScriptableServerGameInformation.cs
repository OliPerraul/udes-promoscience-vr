using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/ServerGameState", order = 1)]
public class ScriptableServerGameInformation : ScriptableObject
{
    ServerGameState gameState;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    public Action gameStateChangedEvent;

    const int tutorialLabyrinthId = 4;

    public int gameRound = 0;

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
        GameState = ServerGameState.Tutorial;

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

        Algorithm algorithm = Algorithm.Tutorial;

        player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, tutorialLabyrinthId, algorithm);
    }

    public void StartNextGameRound()
    {
        gameRound++;
        GameState = ServerGameState.GameRound;

        int[] data = labyrinthData.GetLabyrithDataWithId(((gameRound - 1) % 3) + 1);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if (player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
            {
                Algorithm algorithm = (Algorithm)((player.ServerTeamId + gameRound) % 3) + 1;
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

        Algorithm algorithm = (Algorithm)((player.ServerTeamId + gameRound) % 3) + 1;
        player.serverAlgorithm = algorithm;
        player.serverLabyrinthId = gameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif
        player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, gameRound, algorithm);
    }

    public void StartGameRoundWithSteps(Player player, int[] steps)
    {
        int[] data = labyrinthData.GetLabyrithDataWithId(((gameRound - 1) % 3) + 1);
        int sizeX = labyrinthData.GetLabyrithXLenght();
        int sizeY = labyrinthData.GetLabyrithYLenght();

        Algorithm algorithm = (Algorithm)((player.ServerTeamId + gameRound) % 3) + 1;
        player.serverAlgorithm = algorithm;
        player.serverLabyrinthId = gameRound;

        player.TargetSetGameWithSteps(player.connectionToClient, steps, data, sizeX, sizeY, gameRound, algorithm);
    }

    public void EndRoundOrTutorial()
    {
        GameState = ServerGameState.Intermission;

        for (int i = 0; i < PlayerList.instance.list.Count; i++)
        {
            Player player = PlayerList.instance.GetPlayerWithId(i);

            if (player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing)
            {
                player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextRound);
            }
        }
    }

    public void LoadGameInformationFromDatabase()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        SQLiteUtilities.SetServerGameInformation(this);
#endif

        gameStateChangedEvent += SaveGameInformationToDatabase;//Work only because gameRound is always updated right before gameState
    }

    public void SaveGameInformationToDatabase()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        SQLiteUtilities.InsertServerGameInformation(this);
#endif
    }

    public void ClearGameInformation()
    {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
        SQLiteUtilities.ResetServerGameInformation();
#endif
    }
}

