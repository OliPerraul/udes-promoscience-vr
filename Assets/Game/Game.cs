using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;


namespace UdeS.Promoscience
{
    // Controls the flow of the main activity (rounds etc)
    // A game consists of 3 rounds       

    public enum LevelSelectionMode
    {
        Selected,//Default
        Random,
        Order
    }

    public class ObservableGame : Cirrus.ObservableValue<Game> { }

    [System.Serializable]
    public class Game
    {
        [SerializeField]
        public int Id = 0;

        public Cirrus.ObservableValue<int> Round = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsRoundCompleted = new Cirrus.ObservableValue<bool>();

        [SerializeField]
        protected LevelSelectionMode levelSelectionMode;

        public IData CurrentLabyrinth { get; private set; }

        public List<IData> Labyrinths = new List<IData>();

        [SerializeField]
        private const int tutorialLabyrinthId = 4;

        [SerializeField]
        private Algorithms.Id baseAlgorithmId;


        // Ideally, player should reference a course instead of refering to a course id 
        public List<Course> Courses = new List<Course>();


        public Game(LevelSelectionMode levelOrder)
        {
            levelSelectionMode = levelOrder;

            Id = SQLiteUtilities.GetNextGameID();

            switch (levelOrder)
            {
                case LevelSelectionMode.Order:
                    break;

                case LevelSelectionMode.Random://TODO
                case LevelSelectionMode.Selected:
                    Server.Instance.State.Set(ServerState.LevelSelect);
                    break;
            }
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
                    Labyrinth = CurrentLabyrinth,
                    Algorithm = algorithm,
                    AlgorithmSteps = algorithm.GetAlgorithmSteps(CurrentLabyrinth) // labyrinth)  
                };

                Courses.Add(course);
                player.ServerCourseId = course.Id;

                SQLiteUtilities.InsertPlayerCourse(
                    player.ServerTeamId,
                    player.serverLabyrinthId,
                    (int)player.serverAlgorithm,
                    player.ServerCourseId,
                    Round.Value,
                    Id);
            }
        }

        public void StartNextRound(int labyrinthId)
        {
            StartNextRound(Promoscience.Labyrinths.Resources.Instance.GetLabyrinth(labyrinthId));
        }

        public void StartNextRound(
            IData labyrinth)
        {
            StartRound(
                labyrinth,
                (int)Algorithms.Utils.GetRoundAlgorithm(Round.Value + 1));
        }

        public void StartRound(
            int labyrinthId,
            int algorithmId)
        {
            StartRound(Promoscience.Labyrinths.Resources.Instance.GetLabyrinth(labyrinthId), algorithmId);
        }

        public void StartRound(
            IData labyrinth,
            int algorithmId)
        {
            if (roundState == ServerState.Round)
            {
                Round.Value = (Round.Value % 3) + 1;
            }
                
            CurrentLabyrinth = labyrinth;

            baseAlgorithmId = (Algorithms.Id)algorithmId;

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

                        player.serverAlgorithm = baseAlgorithmId;

                        player.serverLabyrinthId = CurrentLabyrinth.Id;

                        AssignCourse(player);

                        player.TargetSetGame(
                            player.connectionToClient,
                            CurrentLabyrinth.Json,
                            player.serverAlgorithm,
                            Round.Value,
                            false);

                        break;
                }
            }

            DoStartRound();
        }

        [SerializeField]
        protected ServerState roundState;

        public ServerState RoundState => roundState;

        protected virtual void DoStartRound()
        {
            Round.Value = (Round.Value % 3) + 1;

            Labyrinths.Add(CurrentLabyrinth);

            roundState = ServerState.Round;

            Server.Instance.State.Set(ServerState.Round);
        }


        public void StartNextGameRound()
        {
            //Round.Value = (Round.Value % 3) + 1;
            //AlgorithmId = Algorithms.Id.GameRound;
            //StartRoundWithLabyrinth(GameRound);
        }

        public void JoinGameRound(Player player)
        {
            AssignCourse(player);

            player.serverAlgorithm = Algorithms.Utils.GetRoundAlgorithm((int)baseAlgorithmId, player.ServerTeamId);

            player.serverLabyrinthId = CurrentLabyrinth.Id; ;

            player.TargetSetGame(
                player.connectionToClient,
                CurrentLabyrinth.Json,
                player.serverAlgorithm,
                Round.Value,
                false);
        }

        public void JoinGameRoundWithSteps(Player player, int[] steps)
        {
            AssignCourse(player);

            player.serverAlgorithm = Algorithms.Utils.GetRoundAlgorithm((int)baseAlgorithmId, player.ServerTeamId);

            player.serverLabyrinthId = CurrentLabyrinth.Id; ;

            player.TargetSetGameWithSteps(
                player.connectionToClient,
                steps,
                CurrentLabyrinth.Json,
                player.serverAlgorithm,
                Round.Value,
                false); // TODO start with steps tutorial??
        }

        public void EndRoundOrTutorial()
        {
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

            //State = ServerState.LevelSelect;
        }
    }
}