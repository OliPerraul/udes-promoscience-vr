using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/ServerGameState", order = 1)]
    public class ScriptableServerGameInformation : ScriptableObject
    {
        [SerializeField]
        ScriptableLabyrinth labyrinthData;

        public Action gameRoundChangedEvent;
        public Action gameStateChangedEvent;

        const int tutorialLabyrinthId = 4;

        int gameRound = 0;

        ServerGameState gameState;

        public int GameRound
        {
            get
            {
                return gameRound;
            }
            set
            {
                gameRound = value;
                OnGameRoundValueChanged();
            }
        }

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

        public void OnGameRoundValueChanged()
        {
            if (gameRoundChangedEvent != null)
            {
                gameRoundChangedEvent();
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

                if (player.ServerPlayerGameState == ClientGameState.Ready || 
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial || 
                    player.ServerPlayerGameState == ClientGameState.Playing || 
                    player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
                {
                    Utils.Algorithm algorithm = Utils.Algorithm.Tutorial;
                    player.serverAlgorithm = algorithm;
                    player.serverLabyrinthId = tutorialLabyrinthId;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif
                    player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, tutorialLabyrinthId, algorithm);
                }
            }
        }

        public void StartTutorial(Player player)
        {
            int[] data = labyrinthData.GetLabyrithDataWithId(tutorialLabyrinthId);
            int sizeX = labyrinthData.GetLabyrithXLenght();
            int sizeY = labyrinthData.GetLabyrithYLenght();

            Utils.Algorithm algorithm = Utils.Algorithm.Tutorial;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = tutorialLabyrinthId;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif

            player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, tutorialLabyrinthId, algorithm);
        }

        public void StartNextGameRound()
        {
            GameRound = (GameRound % 3) + 1;
            GameState = ServerGameState.GameRound;

            int[] data = labyrinthData.GetLabyrithDataWithId(GameRound);
            int sizeX = labyrinthData.GetLabyrithXLenght();
            int sizeY = labyrinthData.GetLabyrithYLenght();

            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player player = PlayerList.instance.GetPlayerWithId(i);

                if (player.ServerPlayerGameState == ClientGameState.Ready || player.ServerPlayerGameState == ClientGameState.PlayingTutorial || player.ServerPlayerGameState == ClientGameState.Playing || player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
                {
                    Utils.Algorithm algorithm = (Utils.Algorithm)((player.ServerTeamId + GameRound) % 3) + 1;
                    player.serverAlgorithm = algorithm;
                    player.serverLabyrinthId = GameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
                    player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif
                    player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, GameRound, algorithm);
                }
            }
        }

        public void StartGameRound(Player player)
        {
            int[] data = labyrinthData.GetLabyrithDataWithId(GameRound);
            int sizeX = labyrinthData.GetLabyrithXLenght();
            int sizeY = labyrinthData.GetLabyrithYLenght();

            Utils.Algorithm algorithm = (Utils.Algorithm)((player.ServerTeamId + GameRound) % 3) + 1;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = GameRound;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            player.ServerCourseId = SQLiteUtilities.GetNextCourseID();
#endif
            player.TargetSetGame(player.connectionToClient, data, sizeX, sizeY, GameRound, algorithm);
        }

        public void StartGameRoundWithSteps(Player player, int[] steps)
        {
            int[] data = labyrinthData.GetLabyrithDataWithId(GameRound);
            int sizeX = labyrinthData.GetLabyrithXLenght();
            int sizeY = labyrinthData.GetLabyrithYLenght();

            Utils.Algorithm algorithm = (Utils.Algorithm)((player.ServerTeamId + GameRound) % 3) + 1;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = GameRound;

            player.TargetSetGameWithSteps(player.connectionToClient, steps, data, sizeX, sizeY, GameRound, algorithm);
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

        public void BeginPlayback()
        {
            GameState = ServerGameState.ViewingPlayback;
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
}

