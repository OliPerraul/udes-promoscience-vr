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

    public class Game
    {
        public int Id = 0;

        public Cirrus.ObservableValue<int> Round = new Cirrus.ObservableValue<int>();

        public Cirrus.ObservableValue<bool> IsRoundCompleted = new Cirrus.ObservableValue<bool>();

        protected LevelSelectionMode levelSelectionMode;

        public IData CurrentLabyrinth { get; private set; }

        public List<IData> Labyrinths = new List<IData>();

        private const int tutorialLabyrinthId = 4;

        public Algorithms.Id algorithmId;


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

                case LevelSelectionMode.Selected:
                    Server.Instance.State.Set(ServerState.LevelSelect);
                    break;

                case LevelSelectionMode.Random:
                    break;
            }
        }

        public Algorithms.Id GetRoundAlgorithm(int teamId)
        {
            Algorithms.Id id;

            switch (algorithmId)
            {
                case Algorithms.Id.GameRound:
                    id = (Algorithms.Id)((teamId + Round.Value) % 3) + 1;
                    break;

                // default to algorithm set by dropdown
                default:
                    id = algorithmId;
                    break;
            }

            return id;
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

        public void StartRound(
            IData labyrinth,
            int algorithmId)
        {           
            CurrentLabyrinth = labyrinth;

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

        public void StartRound(
            int labyrinthId, 
            int algorithmId)
        {
            Round.Value = (Round.Value % 3) + 1;
            //State = ServerState.GameRound;

            CurrentLabyrinth = Promoscience.Labyrinths.Resources.Instance.GetLabyrinth(labyrinthId);

            Labyrinths.Add(CurrentLabyrinth);

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

        protected virtual void DoStartRound()
        {
            Round.Value = (Round.Value % 3) + 1;

            Labyrinths.Add(CurrentLabyrinth);

            Server.Instance.State.Set(ServerState.Round);
        }


        public void StartNextGameRound()
        {
            Round.Value = (Round.Value % 3) + 1;
            algorithmId = Algorithms.Id.GameRound;
            //StartRoundWithLabyrinth(GameRound);
        }

        public void JoinGameRound(Player player)
        {
            player.serverAlgorithm = GetRoundAlgorithm(player.ServerTeamId);

            player.serverLabyrinthId = CurrentLabyrinth.Id;

            AssignCourse(player);

            player.TargetSetGame(
                player.connectionToClient,
                CurrentLabyrinth.Json,
                player.serverAlgorithm,
                Round.Value,
                false);
        }

        public void JoinGameRoundWithSteps(Player player, int[] steps)
        {
            player.serverAlgorithm = GetRoundAlgorithm(player.ServerTeamId);

            player.serverLabyrinthId = CurrentLabyrinth.Id; ;

            AssignCourse(player);

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
            //State = ServerState.Intermission;

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

        public void StartTutorial()
        {
            Round.Value = tutorialLabyrinthId;

            CurrentLabyrinth =
                Promoscience.Labyrinths.Resources.Instance.GetLabyrinth(tutorialLabyrinthId);

            Labyrinths.Add(CurrentLabyrinth);


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
                            CurrentLabyrinth.Json,
                            algorithm,
                            Round.Value,
                            true);

                        break;
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