//using System;
//using System.Collections;
//using UnityEngine;

//using UdeS.Promoscience.ScriptableObjects;
//using UdeS.Promoscience.Utils;
//using UdeS.Promoscience.Network;
//using System.Collections.Generic;

//namespace UdeS.Promoscience.ScriptableObjects
//{
//    // TODO move to singleton ServerGame
//    // TODO same for client

//    [CreateAssetMenu(fileName = "Data", menuName = "Data/ServerGameState", order = 1)]
//    public class ScriptableServerGameInformation : ScriptableObject
//    {
//        // Ideally, player should reference a course instead of refering to a course id 
//        private Dictionary<int, Course> courses;
                  
//        public ICollection<Course> Courses
//        {
//            get
//            {
//                return courses.Values;
//            }
//        }

//        //[SerializeField]
//        //private Labyrinths.ScriptableLabyrinth labyrinthData;

//        public Labyrinths.IData currentLabyrinth;

//        public Labyrinths.IData CurrentLabyrinth
//        {
//            get
//            {
//                return currentLabyrinth;
//            }
//        }

//        private Replays.Replay replay;

//        public OnCourseEvent OnCourseAddedHandler;

//        public Action gameRoundChangedEvent;

//        public Action gameStateChangedEvent;

//        private const int tutorialLabyrinthId = 4;

//        private int gameRound = 0;

//        private ServerGameState gameState;

//        public void OnEnable()
//        {
//            gameState = ServerGameState.Lobby;
//            courses = new Dictionary<int, Course>();
//        }

//        public int GameRound
//        {
//            get
//            {
//                return gameRound;
//            }
//            set
//            {
//                gameRound = value;
//                OnGameRoundValueChanged();
//            }
//        }

//        public ServerGameState GameState
//        {
//            get
//            {
//                return gameState;
//            }
//            set
//            {
//                gameState = value;
//                OnGameStateValueChanged();
//            }
//        }

//        public void OnGameRoundValueChanged()
//        {
//            if (gameRoundChangedEvent != null)
//            {
//                gameRoundChangedEvent();
//            }
//        }

//        public void OnGameStateValueChanged()
//        {
//            switch (GameState)
//            {
//                case ServerGameState.SimpleReplay:
//                    break;

//                default:
//                    //playbackOptions.Courses.Clear();
//                    break;
//            }
            
//            if (gameStateChangedEvent != null)
//            {
//                gameStateChangedEvent();
//            }
//        }

//        // Try find course ID initiated by a team member
//        // Otherwise assign new course
//        //
//        // Returns true if created a course
//        public void AssignCourse(Player player)//, out Course course)
//        {
//            Course course = null;
//            int courseId = -1;
//            SQLiteUtilities.SetCourseInactive(player.ServerCourseId);

//            // Try to get an active course
//            if (SQLiteUtilities.TryGetActiveCourseId(player.ServerTeamId, out courseId))
//            {
//                player.ServerCourseId = courseId;
//                //return false;
//            }
//            else
//            {
//                var labyrinth = new Labyrinths.Data();

//                course = new Course
//                {
//                    Id = SQLiteUtilities.GetNextCourseID(),
//                    Team = Teams.Resources.Instance.GetScriptableTeamWithId(player.ServerTeamId),
//                    Labyrinth = labyrinth,
//                    Algorithm = Algorithms.Resources.Instance.CreateAlgorithm(player.serverAlgorithm)// labyrinth)                                     
//                };

//                SQLiteUtilities.ReadLabyrinthDataFromId(GameRound, course.Labyrinth);

//                courses.Add(course.Id, course);

//                if (OnCourseAddedHandler != null)
//                {
//                    OnCourseAddedHandler.Invoke(course);
//                }

//                player.ServerCourseId = course.Id;
//                SQLiteUtilities.InsertPlayerCourse(
//                    player.ServerTeamId,
//                    player.serverLabyrinthId,
//                    (int)player.serverAlgorithm,
//                    player.ServerCourseId);

//                //return true;
//            }
//        }

//        public void StartTutorial()
//        {
//            GameRound = tutorialLabyrinthId;
//            GameState = ServerGameState.Tutorial;

//            currentLabyrinth = new Labyrinths.Data();
//            SQLiteUtilities.ReadLabyrinthDataFromId(GameRound, CurrentLabyrinth);

//            for (int i = 0; i < PlayerList.instance.list.Count; i++)
//            {
//                Player player = PlayerList.instance.GetPlayerWithId(i);

//                if (player.ServerPlayerGameState == ClientGameState.Ready || 
//                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial || 
//                    player.ServerPlayerGameState == ClientGameState.Playing || 
//                    player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
//                {
//                    Algorithm algorithm = Algorithm.Tutorial;
//                    player.serverAlgorithm = algorithm;

//                    player.serverLabyrinthId = tutorialLabyrinthId;

//                    AssignCourse(player);

//                    // TODO send course json over??
//                    player.TargetSetGame(
//                        player.connectionToClient, 
//                        currentLabyrinth.data, 
//                        currentLabyrinth.sizeX, 
//                        currentLabyrinth.sizeY, 
//                        tutorialLabyrinthId, 
//                        algorithm);
//                }
//            }
//        }

//        public void StartTutorial(Player player)
//        {
//            Algorithm algorithm = Algorithm.Tutorial;
//            player.serverAlgorithm = algorithm;
//            player.serverLabyrinthId = tutorialLabyrinthId;

//            AssignCourse(player);

//            player.TargetSetGame(
//                player.connectionToClient, 
//                currentLabyrinth.data, 
//                currentLabyrinth.sizeX, 
//                currentLabyrinth.sizeY, 
//                tutorialLabyrinthId, 
//                algorithm);
//        }

//        public void StartNextGameRound()
//        {
//            GameRound = (GameRound % 3) + 1;
//            GameState = ServerGameState.GameRound;

//            currentLabyrinth = new Labyrinths.Data();
//            SQLiteUtilities.ReadLabyrinthDataFromId(GameRound, CurrentLabyrinth);

//            for (int i = 0; i < PlayerList.instance.list.Count; i++)
//            {
//                Player player = PlayerList.instance.GetPlayerWithId(i);

//                if (player.ServerPlayerGameState == ClientGameState.Ready || 
//                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial || 
//                    player.ServerPlayerGameState == ClientGameState.Playing || 
//                    player.ServerPlayerGameState == ClientGameState.WaitingForNextRound)
//                {
//                    Algorithm algorithm = (Algorithm)((player.ServerTeamId + GameRound) % 3) + 1;
//                    player.serverAlgorithm = algorithm;
//                    player.serverLabyrinthId = GameRound;

//                    AssignCourse(player);

//                    player.TargetSetGame(
//                        player.connectionToClient, 
//                        CurrentLabyrinth.data, 
//                        CurrentLabyrinth.sizeX, 
//                        CurrentLabyrinth.sizeY, 
//                        GameRound, 
//                        algorithm);
//                }
//            }
//        }

//        public void StartGameRound(Player player)
//        {
//            Algorithm algorithm = (Algorithm)((player.ServerTeamId + GameRound) % 3) + 1;
//            player.serverAlgorithm = algorithm;
//            player.serverLabyrinthId = GameRound;

//            AssignCourse(player);

//            player.TargetSetGame(
//                player.connectionToClient, 
//                CurrentLabyrinth.data, 
//                CurrentLabyrinth.sizeX,
//                CurrentLabyrinth.sizeY,
//                GameRound,
//                algorithm);
//        }

//        public void StartGameRoundWithSteps(Player player, int[] steps)
//        {

//            Algorithm algorithm = (Algorithm)((player.ServerTeamId + GameRound) % 3) + 1;
//            player.serverAlgorithm = algorithm;
//            player.serverLabyrinthId = GameRound;

//            AssignCourse(player);

//            player.TargetSetGameWithSteps(
//                player.connectionToClient, 
//                steps, CurrentLabyrinth.data, 
//                CurrentLabyrinth.sizeX, 
//                CurrentLabyrinth.sizeY, 
//                GameRound, 
//                algorithm);
//        }

//        public void EndRoundOrTutorial()
//        {
//            GameState = ServerGameState.Intermission;

//            for (int i = 0; i < PlayerList.instance.list.Count; i++)
//            {
//                Player player = PlayerList.instance.GetPlayerWithId(i);
//                SQLiteUtilities.SetCourseInactive(player.ServerCourseId);

//                if (player.ServerPlayerGameState == ClientGameState.PlayingTutorial || 
//                    player.ServerPlayerGameState == ClientGameState.Playing)
//                {
//                    player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextRound);
//                }
//            }
//        }

//        public void BeginAdvancedReplay()
//        {
//            // TODO: Player should not refer to courseId anymore, maybe simply refer to course obj?               
//            foreach (Player player in PlayerList.instance.list)
//            {
//                // Tell clients to pay attention
//                if (player.ServerPlayerGameState == ClientGameState.WaitingReplay ||
//                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
//                    player.ServerPlayerGameState == ClientGameState.ViewingGlobalReplay ||
//                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
//                    player.ServerPlayerGameState == ClientGameState.Playing)
//                {
//                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ViewingGlobalReplay);
//                }
//            }

//            GameState = ServerGameState.AdvancedReplay;
//        }


//        public void BeginSimpleReplay()
//        {         
//             // TODO: Player should not refer to courseId anymore, maybe simply refer to course obj?               
//            foreach (Player player in PlayerList.instance.list)
//            {
//                // Tell clients to pay attention
//                if (player.ServerPlayerGameState == ClientGameState.WaitingReplay ||
//                    player.ServerPlayerGameState == ClientGameState.ViewingLocalReplay ||
//                    player.ServerPlayerGameState == ClientGameState.ViewingGlobalReplay ||
//                    player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
//                    player.ServerPlayerGameState == ClientGameState.Playing)
//                {
//                    player.TargetSetGameState(player.connectionToClient, ClientGameState.ViewingGlobalReplay);
//                }
//            }

//            //CourseData courseData;
//            Queue<int> steps;
//            Queue<string> stepValues; //jsons
//            foreach(Course course in Courses)
//            {
//                SQLiteUtilities.SetCourseInactive(course.Id);
//                SQLiteUtilities.GetPlayerStepsForCourse(course.Id, out steps, out stepValues);

//                // Add sentinel values
//                // TODO put somewhere else? Maybe from the headset?
//                steps.Enqueue((int)GameAction.EndMovement);
//                stepValues.Enqueue(JsonUtility.ToJson(new ActionValue()));

//                course.Actions = steps.ToArray();
//                course.ActionValues = stepValues.ToArray();
//            }

//            // Begin playback server
//            GameState = ServerGameState.SimpleReplay;
//        }

//        public void LoadGameInformationFromDatabase()
//        {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//            SQLiteUtilities.SetServerGameInformation(this);
//#endif
//            gameStateChangedEvent += SaveGameInformationToDatabase;//Work only because gameRound is always updated right before gameState
//        }

//        public void SaveGameInformationToDatabase()
//        {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//            SQLiteUtilities.InsertServerGameInformation(this);
//#endif
//        }

//        public void ClearGameInformation()
//        {
//#if UNITY_EDITOR || UNITY_STANDALONE_WIN
//            SQLiteUtilities.ResetServerGameInformation();
//#endif
//        }
//    }
//}

