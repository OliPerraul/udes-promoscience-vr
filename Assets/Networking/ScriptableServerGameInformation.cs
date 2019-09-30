using System;
using System.Collections;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience.Network;
using System.Collections.Generic;

namespace UdeS.Promoscience.ScriptableObjects
{
    [CreateAssetMenu(fileName = "Data", menuName = "Data/ServerGameState", order = 1)]
    public class ScriptableServerGameInformation : ScriptableObject
    {
        [SerializeField]
        ScriptableLabyrinth labyrinthData;

        [SerializeField]
        private ScriptableTeamList teams;

        [SerializeField]
        private Replay.ScriptableReplayOptions playbackOptions;

        // Ideally, player should reference a course instead of refering to a course id 
        public Dictionary<int, CourseData> Courses;

        public Action gameRoundChangedEvent;

        public Action gameStateChangedEvent;

        const int tutorialLabyrinthId = 4;

        int gameRound = 0;

        ServerGameState gameState;

        public void OnEnable()
        {
            gameState = ServerGameState.Lobby;
            Courses = new Dictionary<int, CourseData>();

            foreach (ScriptableTeam team in teams.Teams)
            {
                SQLiteUtilities.InsertPlayerTeam(
                    team.TeamId,
                    team.TeamName,
                    team.TeamColor.ToString(),                    
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }
        }

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
            switch (GameState)
            {
                case ServerGameState.ViewingPlayback:
                    break;

                default:
                    playbackOptions.Courses.Clear();
                    break;
            }
            
            if (gameStateChangedEvent != null)
            {
                gameStateChangedEvent();
            }
        }

        // TODO set course active false when finished
        // Try find course ID initiated by a team member
        // Otherwise assign new course
        public void AssignCourse(Player player)
        {
            int courseId = -1;
            SQLiteUtilities.SetCourseInactive(player.ServerCourseId);

            if (SQLiteUtilities.TryGetCourseId(player.ServerTeamId, out courseId))
            {
                player.ServerCourseId = courseId;
            }
            else
            {
                CourseData course = new CourseData();
                course.Id = SQLiteUtilities.GetNextCourseID();
                course.Team = teams.GetScriptableTeamWithId(player.ServerTeamId);
                course.Algorithm = player.serverAlgorithm;
                Courses.Add(course.Id, course);

                player.ServerCourseId = course.Id;
                SQLiteUtilities.InsertPlayerCourse(
                    player.ServerTeamId,
                    player.serverLabyrinthId,
                    (int)player.serverAlgorithm,
                    player.ServerCourseId);
            }
        }

        //// TODO set course active false when finished
        //// Try find course ID initiated by a team member
        //// Otherwise assign new course
        //public void RemoveCourse(int course)
        //{
        //    SQLiteUtilities.SetCourseInactive(course);
        //    Courses.Remove(course);
        //}


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

                    AssignCourse(player);

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

            AssignCourse(player);

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

                    AssignCourse(player);

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

            AssignCourse(player);

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

            AssignCourse(player);

            player.TargetSetGameWithSteps(player.connectionToClient, steps, data, sizeX, sizeY, GameRound, algorithm);
        }

        public void EndRoundOrTutorial()
        {
            GameState = ServerGameState.Intermission;

            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player player = PlayerList.instance.GetPlayerWithId(i);
                SQLiteUtilities.SetCourseInactive(player.ServerCourseId);

                if (player.ServerPlayerGameState == ClientGameState.PlayingTutorial || 
                    player.ServerPlayerGameState == ClientGameState.Playing)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextRound);
                }
            }
        }

        public void BeginPlayback()
        {         
             // TODO: Player should not refer to courseId anymore, maybe simply refer to course obj?               
            foreach (Player player in PlayerList.instance.list)
            {
                // Tell clients to pay attention
                if (player.ServerPlayerGameState == ClientGameState.WaitingReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingGlobalReplay ||
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
                    player.ServerPlayerGameState == ClientGameState.Playing)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ViewingGlobalReplay);
                }
            }

            //CourseData courseData;
            Queue<int> steps;
            Queue<string> stepValues; //jsons
            foreach(CourseData course in Courses.Values)
            {
                SQLiteUtilities.SetCourseInactive(course.Id);
                SQLiteUtilities.GetPlayerStepsForCourse(course.Id, out steps, out stepValues);
                course.Actions = steps.ToArray();
                course.ActionValues = stepValues.ToArray();
                playbackOptions.Courses.Add(course);
            }

            Courses.Clear();

            // Begin playback server
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

