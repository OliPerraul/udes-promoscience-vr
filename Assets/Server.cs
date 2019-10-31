

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
        private Replays.Replay replay;

        public Replays.ScriptableController replayController;

        public class LabyrinthValues
        {
            public IData CurrentData { get; set; }

            public Labyrinth Currentlabyrinth;

            public Labyrinth CurrentLabyrinth { get; set; }

            public ICollection<IData> data = new List<IData>();

            public ICollection<IData> Data
            {
                get
                {
                    if (data == null || data.Count == 0)
                    {
                        data = SQLiteUtilities.GetLabyrinths();
                    }

                    return data;
                }
            }

            public ICollection<Labyrinth> Labyrinths
            {
                get
                {
                    return IdPairs.Values;
                }
            }

            private Dictionary<int, Labyrinth> labyrinths = new Dictionary<int, Labyrinth>();

            public IDictionary<int, Labyrinth> IdPairs
            {
                get
                {
                    if (labyrinths == null)
                        labyrinths = new Dictionary<int, Labyrinth>();
                    return labyrinths;
                }
            }

            public void Clear()
            {
                if (data != null)
                    data.Clear();

                if (labyrinths != null)
                    labyrinths.Clear();
            }
        }

        public LabyrinthValues Labyrinths = new LabyrinthValues();


        // Ideally, player should reference a course instead of refering to a course id 
        public List<Course> Courses;

        private Dictionary<int, Course> idCoursePairs = new Dictionary<int, Course>();

        public Dictionary<int, Course> IdCoursePairs { get { return idCoursePairs; } }

        public OnCourseEvent OnCourseAddedHandler;

        private int gameRound = 0;


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

        private ServerGameState gameState;


        public Action gameRoundChangedEvent;

        public Action gameStateChangedEvent;

        private const int tutorialLabyrinthId = 4;


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
        }

        public Replays.LabyrinthReplay CurrentReplay;



        public void ClearLabyrinths()
        {
            Labyrinths.Clear();
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

        public void BeginInstantReplay()
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

            GameState = ServerGameState.InstantReplay;

            GameState = ServerGameState.AdvancedReplay;

            Courses = SQLiteUtilities.GetSessionCourses();

            Labyrinth labyrinth = ScriptableResources.Instance
                .GetLabyrinthTemplate(Labyrinths.CurrentData)
                .Create(Labyrinths.CurrentData);

            labyrinth.GenerateLabyrinthVisual();

            CurrentReplay = new Replays.LabyrinthReplay(replayController, labyrinth);

            CurrentReplay.Start();

        }

        public void BeginAdvancedReplay(IData labyrinth)
        {
            GameState = ServerGameState.AdvancedReplay;

            Courses = SQLiteUtilities.GetSessionCourses();// ForLabyrinth(lab.Id);

            Labyrinths.CurrentData = labyrinth;

            Labyrinths.CurrentLabyrinth = ScriptableResources.Instance
                .GetLabyrinthTemplate(labyrinth)
                .Create(labyrinth);

            Labyrinths.CurrentLabyrinth.GenerateLabyrinthVisual();

            CurrentReplay = new Replays.LabyrinthReplay(replayController, Labyrinths.CurrentLabyrinth);
                       
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

                SQLiteUtilities.ReadLabyrinthDataFromId(Labyrinths.CurrentData.currentId, course.Labyrinth);

                IdCoursePairs.Add(course.Id, course);

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

            Labyrinths.CurrentData = new Data();
            SQLiteUtilities.ReadLabyrinthDataFromId(GameRound, Labyrinths.CurrentData);

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
                        Labyrinths.CurrentData.data,
                        Labyrinths.CurrentData.sizeX,
                        Labyrinths.CurrentData.sizeY,
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
                Labyrinths.CurrentData.data,
                Labyrinths.CurrentData.sizeX,
                Labyrinths.CurrentData.sizeY,
                tutorialLabyrinthId,
                algorithm,
                isTutorial: true);
        }

        public void StartGameWithLabyrinth(int labyrinthId)
        {
            GameRound = (GameRound % 3) + 1;
            GameState = ServerGameState.GameRound;

            Labyrinths.CurrentData = new Data();
            SQLiteUtilities.ReadLabyrinthDataFromId(labyrinthId, Labyrinths.CurrentData);

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
                    player.serverLabyrinthId = Labyrinths.CurrentData.currentId;

                    AssignCourse(player);

                    player.TargetSetGame(
                        player.connectionToClient,
                        Labyrinths.CurrentData.data,
                        Labyrinths.CurrentData.sizeX,
                        Labyrinths.CurrentData.sizeY,
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
            player.serverLabyrinthId = Labyrinths.CurrentData.currentId;

            AssignCourse(player);

            player.TargetSetGame(
                player.connectionToClient,
                Labyrinths.CurrentData.data,
                Labyrinths.CurrentData.sizeX,
                Labyrinths.CurrentData.sizeY,
                Labyrinths.CurrentData.currentId,
                algorithm,
                false);
        }

        public void StartGameRoundWithSteps(Player player, int[] steps)
        {
            Algorithms.Id algorithm = (Algorithms.Id)((player.ServerTeamId + GameRound) % 3) + 1;
            player.serverAlgorithm = algorithm;
            player.serverLabyrinthId = Labyrinths.CurrentData.currentId; ;

            AssignCourse(player);

            player.TargetSetGameWithSteps(
                player.connectionToClient,
                steps, Labyrinths.CurrentData.data,
                Labyrinths.CurrentData.sizeX,
                Labyrinths.CurrentData.sizeY,
                Labyrinths.CurrentData.currentId,
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

