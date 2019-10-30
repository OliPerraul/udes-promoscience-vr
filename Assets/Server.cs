

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
using System;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.Labyrinths;

namespace UdeS.Promoscience
{
    public class Server : MonoBehaviour
    {
        public Labyrinths.IData CurrentLabyrinth
        {
            get
            {
                return currentLabyrinth;
            }
        }

        private Replays.Replay replay;

        public Replays.ScriptableController replayController;

        public OnCourseEvent OnCourseAddedHandler;

        public Action gameRoundChangedEvent;

        public Action gameStateChangedEvent;

        private const int tutorialLabyrinthId = 4;

        private int gameRound = 0;

        private ServerGameState gameState;

        // Ideally, player should reference a course instead of refering to a course id 
        public List<Course> Courses;

        private Dictionary<int, Course> idCoursePairs;

        public Labyrinths.IData currentLabyrinth;


        private static Server instance = null;

        public static Server Instance
        {
            get
            {
                if (instance == null)
                    instance = FindObjectOfType<Server>();

                return instance;
            }
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
            instance = this;
            DontDestroyOnLoad(gameObject);

            // When server starts set all previous course to innactive
            var sessionCourses = SQLiteUtilities.GetSessionCourses();
            foreach (Course c in sessionCourses)
            {
                SQLiteUtilities.SetCourseFinished(c.Id);
            }

            // Store all the teams in the DB
            // TODO remove, replace with 'Resources' asset
            foreach (Teams.ScriptableTeam team in Teams.Resources.Instance.Teams)
            {
                SQLiteUtilities.InsertPlayerTeam(
                    team.TeamId,
                    team.TeamName,
                    team.TeamColor.ToString(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            GameState = ServerGameState.Lobby;

            idCoursePairs = new Dictionary<int, Course>();
            labyrinthsData = new List<Labyrinths.IData>();
            labyrinths = new Dictionary<int, Labyrinths.Labyrinth>();
        }

        public Replays.LabyrinthReplay CurrentReplay;

        public ICollection<IData> labyrinthsData;

        public ICollection<IData> LabyrinthsData
        {
            get
            {
                if (labyrinthsData == null || labyrinthsData.Count == 0)
                {
                    labyrinthsData = SQLiteUtilities.GetLabyrinths();
                }

                return labyrinthsData;
            }
        }

        public ICollection<Labyrinths.Labyrinth> Labyrinths
        {
            get
            {
                return IdLabyrinthPairs.Values;
            }
        }

        private Dictionary<int, Labyrinths.Labyrinth> labyrinths;

        public IDictionary<int, Labyrinths.Labyrinth> IdLabyrinthPairs
        {
            get
            {
                if (labyrinths == null)
                    labyrinths = new Dictionary<int, Labyrinths.Labyrinth>();
                return labyrinths;
            }
        }

        public void ClearLabyrinths()
        {
            if (labyrinthsData != null)
                labyrinthsData.Clear();

            if (labyrinths != null)
                labyrinths.Clear();
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
                case ServerGameState.SimpleReplay:
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

        public void BeginSimpleReplay()
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

            GameState = ServerGameState.SimpleReplay;

            GameState = ServerGameState.AdvancedReplay;

            Courses = SQLiteUtilities.GetSessionCourses();

            Labyrinth labyrinth = ScriptableResources.Instance
                .GetLabyrinth(CurrentLabyrinth)
                .Create(CurrentLabyrinth);

            labyrinth.GenerateLabyrinthVisual();

            CurrentReplay = new Replays.LabyrinthReplay(replayController, labyrinth);

            CurrentReplay.Start();


        }

        public void BeginAdvancedReplay(Labyrinth labyrinth)
        {
            foreach (Labyrinth l in Instance.Labyrinths)
            {
                l.gameObject.SetActive(false);
            }

            labyrinth.gameObject.SetActive(true);

            GameState = ServerGameState.AdvancedReplay;

            Courses = SQLiteUtilities.GetSessionCourses();// ForLabyrinth(lab.Id);

            CurrentReplay = new Replays.LabyrinthReplay(replayController, labyrinth);
                       
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
                //return false;
            }
            else
            {
                var labyrinth = new Data();

                course = new Course
                {
                    Id = SQLiteUtilities.GetNextCourseID(),
                    Team = Teams.Resources.Instance.GetScriptableTeamWithId(player.ServerTeamId),
                    Labyrinth = labyrinth,
                    Algorithm = Algorithms.Resources.Instance.CreateAlgorithm(player.serverAlgorithm)// labyrinth)                                     
                };

                SQLiteUtilities.ReadLabyrinthDataFromId(CurrentLabyrinth.currentId, course.Labyrinth);

                idCoursePairs.Add(course.Id, course);

                if (OnCourseAddedHandler != null)
                {
                    OnCourseAddedHandler.Invoke(course);
                }

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

            currentLabyrinth = new Data();
            SQLiteUtilities.ReadLabyrinthDataFromId(GameRound, CurrentLabyrinth);

            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player player = PlayerList.instance.GetPlayerWithId(i);

                if (player.ServerPlayerGameState == ClientGameState.Ready ||
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
                    player.ServerPlayerGameState == ClientGameState.Playing ||
                    player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
                {
                    Algorithms.Id algorithm = Algorithms.Id.Tutorial;
                    player.serverAlgorithm = algorithm;

                    player.serverLabyrinthId = tutorialLabyrinthId;

                    AssignCourse(player);

                    // TODO send course json over??
                    player.TargetSetGame(
                        player.connectionToClient,
                        currentLabyrinth.data,
                        currentLabyrinth.sizeX,
                        currentLabyrinth.sizeY,
                        tutorialLabyrinthId,
                        algorithm,
                        true);                   
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
                currentLabyrinth.data,
                currentLabyrinth.sizeX,
                currentLabyrinth.sizeY,
                tutorialLabyrinthId,
                algorithm,
                isTutorial: true);
        }

        public void StartGameWithLabyrinth(int labyrinthId)
        {
            GameRound = (GameRound % 3) + 1;
            GameState = ServerGameState.GameRound;

            currentLabyrinth = new Labyrinths.Data();
            SQLiteUtilities.ReadLabyrinthDataFromId(labyrinthId, CurrentLabyrinth);

            for (int i = 0; i < PlayerList.instance.list.Count; i++)
            {
                Player player = PlayerList.instance.GetPlayerWithId(i);

                if (player.ServerPlayerGameState == ClientGameState.Ready ||
                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
                    player.ServerPlayerGameState == ClientGameState.Playing ||
                    player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
                {
                    Algorithms.Id algorithm = (Algorithms.Id)((player.ServerTeamId + GameRound) % 3) + 1;
                    player.serverAlgorithm = algorithm;
                    player.serverLabyrinthId = CurrentLabyrinth.currentId;

                    AssignCourse(player);

                    player.TargetSetGame(
                        player.connectionToClient,
                        CurrentLabyrinth.data,
                        CurrentLabyrinth.sizeX,
                        CurrentLabyrinth.sizeY,
                        labyrinthId,
                        algorithm,
                        false);
                }
            }
        }

        public void StartNextGameRound()
        {
            GameRound = (GameRound % 3) + 1;

            StartGameWithLabyrinth(GameRound);
        }

        public void StartGameRound(Player player)
        {
            Algorithms.Id algorithm = (Algorithms.Id)((player.ServerTeamId + GameRound) % 3) + 1;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = CurrentLabyrinth.currentId;

            AssignCourse(player);

            player.TargetSetGame(
                player.connectionToClient,
                CurrentLabyrinth.data,
                CurrentLabyrinth.sizeX,
                CurrentLabyrinth.sizeY,
                CurrentLabyrinth.currentId,
                algorithm,
                false);
        }

        public void StartGameRoundWithSteps(Player player, int[] steps)
        {
            Algorithms.Id algorithm = (Algorithms.Id)((player.ServerTeamId + GameRound) % 3) + 1;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = CurrentLabyrinth.currentId; ;

            AssignCourse(player);

            player.TargetSetGameWithSteps(
                player.connectionToClient,
                steps, CurrentLabyrinth.data,
                CurrentLabyrinth.sizeX,
                CurrentLabyrinth.sizeY,
                CurrentLabyrinth.currentId,
                algorithm,
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

