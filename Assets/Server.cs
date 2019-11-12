//using UdeS.Promoscience.Utils;
using System;
using System.Collections.Generic;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using UnityEngine.SceneManagement;

using UnityEngine;
using Cirrus;

namespace UdeS.Promoscience
{
    public class Server : Cirrus.BaseSingleton<Server>
    {
        private Replays.Replay replay;

        public Replays.ControllerAsset replayController;

        public LabyrinthManager Labyrinths = new LabyrinthManager();
        
        [SerializeField]
        private GameRoundManagerAsset gameRoundManager;

        // Ideally, player should reference a course instead of refering to a course id 
        public List<Course> Courses;

        private Dictionary<int, Course> idCoursePairs = new Dictionary<int, Course>();

        public Dictionary<int, Course> IdCoursePairs { get { return idCoursePairs; } }

        private ServerGameState gameState;
        
        public Action gameStateChangedEvent;

        private const int tutorialLabyrinthId = 4;

        public Algorithms.Id algorithmId;

        public Algorithms.Id GetRoundAlgorithm(int teamId)
        {
            Algorithms.Id id;

            switch (algorithmId)
            {
                case Algorithms.Id.GameRound:
                    id = (Algorithms.Id)((teamId + GameRound) % 3) + 1;
                    break;
                
                    // default to algorithm set by dropdown
                default:
                    id = algorithmId;
                    break;
            }

            return id;
        }

        public static bool IsApplicationServer
        {
            get
            {
                return Instance == null;
            }
        }

        public void Awake()
        {
            //if (Exists)
            //{
            //    DestroyImmediate(gameObject);
            //    return;
            //}

            //DontDestroyOnLoad(gameObject);

            // When server starts set all previous course to innactive
            var sessionCourses = SQLiteUtilities.GetSessionCourses();
            foreach (Course c in sessionCourses)
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            // Store all the teams in the DB
            // TODO remove, replace with 'Resources' asset
            foreach (Teams.TeamResource team in Teams.Resources.Instance.Teams)
            {
                SQLiteUtilities.InsertPlayerTeam(
                    team.TeamId,
                    team.TeamName,
                    team.TeamColor.ToString(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            GameState = ServerGameState.Lobby;                       
        }

        public Replays.Replay CurrentReplay;



        public void ClearLabyrinths()
        {
            Labyrinths.Clear();
        }

        public int GameRound
        {
            get
            {
                return gameRoundManager.Round.Value;
            }
            set
            {
                gameRoundManager.Round.Value = value;
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

        public void OnGameStateValueChanged()
        {
            switch (GameState)
            {
                case ServerGameState.InstantReplay:
                    break;

                default:
                    //playbackOptions.Courses.Clear();
                    break;
            }

            if (gameStateChangedEvent != null)
            {
                gameStateChangedEvent();
            }
        }

        // Set algorithm  using the UI option
        public void SetAlgorithm(Algorithms.Id alg)
        { 
            algorithmId = alg;
        }

        public void StartInstantReplay()
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
                    player.TargetSetGameState(
                        player.connectionToClient, 
                        ClientGameState.ViewingGlobalReplay);                    
                }
            }

            GameState = ServerGameState.InstantReplay;

            Courses = SQLiteUtilities.GetSessionCoursesForLabyrinth(Labyrinths.CurrentData.Id);

            CurrentReplay = new Replays.InstantReplay(replayController, Labyrinths.CurrentData);

            CurrentReplay.Start();
        }

        public void StartAdvancedReplay(IData labyrinth)
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
                    player.TargetSetGameState(
                        player.connectionToClient,
                        ClientGameState.ViewingGlobalReplay);
                }
            }

            GameState = ServerGameState.AdvancedReplay;
            
            Courses = SQLiteUtilities.GetSessionCoursesForLabyrinth(labyrinth.Id);

            Labyrinths.CurrentData = labyrinth;

            CurrentReplay = new Replays.Replay(
                replayController, 
                Labyrinths.CurrentData);
                       
            CurrentReplay.Start();
        }

        // Try find course ID initiated by a team member
        // Otherwise assign new course
        //
        // Returns true if created a course
        public void AssignCourse(Player player)//, out Course course)
        {                 
            Course course = null;
            int courseId = -1;
            SQLiteUtilities.SetCourseInactive(player.ServerCourseId);

            // Try to get an active course
            if (SQLiteUtilities.TryGetActiveCourseId(player.ServerTeamId, out courseId))
            {
                player.ServerCourseId = courseId;
            }
            else
            {
                var algorithm = Algorithms.Resources.Instance.GetAlgorithm(player.serverAlgorithm);

                course = new Course
                {
                    Id = SQLiteUtilities.GetNextCourseID(),
                    Team = Teams.Resources.Instance.GetScriptableTeamWithId(player.ServerTeamId),
                    Labyrinth = Labyrinths.CurrentData,
                    Algorithm = algorithm,
                    AlgorithmSteps = algorithm.GetAlgorithmSteps(Labyrinths.CurrentData) // labyrinth)  

                };

                IdCoursePairs.Add(course.Id, course);

                player.ServerCourseId = course.Id;

                SQLiteUtilities.InsertPlayerCourse(
                    player.ServerTeamId,
                    player.serverLabyrinthId,
                    (int)player.serverAlgorithm,
                    player.ServerCourseId);
            }
        }

        public void StartTutorial()
        {
            GameRound = tutorialLabyrinthId;
            GameState = ServerGameState.Tutorial;

            Labyrinths.CurrentData =
                Promoscience.Labyrinths
                .Resources.Instance.GetLabyrinthData(tutorialLabyrinthId);

            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player player = PlayerList.instance.GetPlayerWithId(i);

                switch (player.ServerPlayerGameState)
                {
                    case ClientGameState.Ready:
                    case ClientGameState.PlayingTutorial:
                    case ClientGameState.Playing:
                    case ClientGameState.ViewingGlobalReplay:
                    case ClientGameState.ViewingLocalReplay:
                    case ClientGameState.WaitingForNextRound:

                        Algorithms.Id algorithm = Algorithms.Id.Tutorial;
                        player.serverAlgorithm = algorithm;

                        player.serverLabyrinthId = tutorialLabyrinthId;

                        AssignCourse(player);

                        // TODO send course json over??
                        player.TargetSetGame(
                            player.connectionToClient,
                            Labyrinths.CurrentData.Json,
                            algorithm,
                            true);

                        break;
                }
            }
        }

        public void StartTutorial(Player player)
        {
            Algorithms.Id algorithm = Algorithms.Id.Tutorial;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = tutorialLabyrinthId;

            AssignCourse(player);

            player.TargetSetGame(
                player.connectionToClient,
                Labyrinths.CurrentData.Json,
                algorithm,
                isTutorial: true);
        }

        public void StartGameWithLabyrinth(int labyrinthId)
        {
            GameRound = (GameRound % 3) + 1;
            GameState = ServerGameState.GameRound;

            Labyrinths.CurrentData = 
                Promoscience.Labyrinths
                .Resources.Instance.GetLabyrinthData(labyrinthId);
                       

            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player player = PlayerList.instance.GetPlayerWithId(i);

                switch (player.ServerPlayerGameState)
                {
                    case ClientGameState.Ready:
                    case ClientGameState.PlayingTutorial:
                    case ClientGameState.Playing:
                    case ClientGameState.ViewingGlobalReplay:
                    case ClientGameState.ViewingLocalReplay:
                    case ClientGameState.WaitingForNextRound:                                               

                        player.serverAlgorithm = GetRoundAlgorithm(player.ServerTeamId);

                        player.serverLabyrinthId = Labyrinths.CurrentData.Id;

                        AssignCourse(player);

                        player.TargetSetGame(
                            player.connectionToClient,
                            Labyrinths.CurrentData.Json,
                            player.serverAlgorithm,
                            false);

                        break;
                }
            }
        }

        public void StartNextGameRound()
        {
            GameRound = (GameRound % 3) + 1;
            algorithmId = Algorithms.Id.GameRound;
            StartGameWithLabyrinth(GameRound);
        }

        public void StartGameRound(Player player)
        {

            player.serverAlgorithm = GetRoundAlgorithm(player.ServerTeamId);

            player.serverLabyrinthId = Labyrinths.CurrentData.Id;

            AssignCourse(player);

            player.TargetSetGame(
                player.connectionToClient,
                Labyrinths.CurrentData.Json,
                player.serverAlgorithm,
                false);
        }

        public void StartGameRoundWithSteps(Player player, int[] steps)
        {
            player.serverAlgorithm = GetRoundAlgorithm(player.ServerTeamId);

            player.serverLabyrinthId = Labyrinths.CurrentData.Id; ;

            AssignCourse(player);

            player.TargetSetGameWithSteps(
                player.connectionToClient,
                steps, 
                Labyrinths.CurrentData.Json,
                player.serverAlgorithm,
                false); // TODO start with steps tutorial??
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
                    player.TargetSetEndRoundOrTutorial(player.connectionToClient);
                }
            }
        }

        public void LevelSelect()
        {
            // TODO: Player should not refer to courseId anymore, maybe simply refer to course obj?               
            foreach (Player player in PlayerList.instance.list)
            {
                // Tell clients to pay attention
                if (player.ServerPlayerGameState == ClientGameState.WaitingReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
                    player.ServerPlayerGameState == ClientGameState.ViewingGlobalReplay ||
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
                    player.ServerPlayerGameState == ClientGameState.Playing)
                {
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ViewingGlobalReplay);
                }
            }

            GameState = ServerGameState.LabyrinthSelect;
        }

        public void LoadGameInformationFromDatabase()
        {
            //SQLiteUtilities.SetServerGameInformation(this);
            //gameStateChangedEvent += SaveGameInformationToDatabase;//Work only because gameRound is always updated right before gameState
        }

        public void SaveGameInformationToDatabase()
        {
            //SQLiteUtilities.InsertServerGameInformation(this);

            
        }

        public void ClearGameInformation()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            //SQLiteUtilities.ResetServerGameInformation();
#endif
        }

    
    }
}

