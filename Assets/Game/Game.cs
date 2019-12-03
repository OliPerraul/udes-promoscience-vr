using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using Cirrus.Extensions;
using System;

namespace UdeS.Promoscience
{
    // Controls the flow of the main activity (rounds etc)
    // A game consists of 3 rounds       

    public enum LevelSelectionMode
    {
        Selected,//Default
        Predefined
    }

    public class ObservableGame : Cirrus.ObservableValue<Game> { }

    [System.Serializable]
    public class Game
    {
        [SerializeField]
        private Level[] predefinedLevels;

        [SerializeField]
        public int Id = 0;

        [SerializeField]
        private GameAsset asset;

        public Cirrus.ObservableValue<int> Round => asset.Round;

        public Cirrus.ObservableValue<bool> IsRoundCompleted => asset.IsRoundCompleted;

        [SerializeField]
        protected LevelSelectionMode levelSelectionMode;

        public IData CurrentLabyrinth { get; private set; }

        public List<IData> Labyrinths = new List<IData>();

        [SerializeField]
        private const int tutorialLabyrinthId = 4;

        [SerializeField]
        private Algorithms.Id baseAlgorithmId;


        [SerializeField]
        protected ServerState roundState;

        public ServerState RoundState => roundState;


        // Ideally, player should reference a course instead of refering to a course id 
        public List<Course> Courses = new List<Course>();


        public Game(GameAsset asset)
        {
            this.asset = asset;

            roundState = ServerState.Round;

            levelSelectionMode = LevelSelectionMode.Selected;
        }

        public Game(GameAsset asset, Level[] predefinedLevels)
        {
            this.predefinedLevels = predefinedLevels;

            this.asset = asset;

            roundState = ServerState.Round;

            levelSelectionMode = LevelSelectionMode.Predefined;
        }

        public void Start()
        {
            Id = SQLiteUtilities.GetNextGameID();

            Round.Value = -1;

            StartNextRound();            
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
                    asset.Round.Value,
                    Id);
            }
        }

        public void Stop()
        {
            Server.Instance.State.Set(ServerState.ThanksForPlaying);
        }

        public void StartNextRound()
        {
            if (levelSelectionMode == LevelSelectionMode.Selected)
            {
                Server.Instance.State.Set(ServerState.LevelSelect);
            }
            else
            {

                StartNextRound(
                predefinedLevels[(Round.Value + 1).Mod(predefinedLevels.Length)].Labyrinth,
                predefinedLevels[(Round.Value + 1).Mod(predefinedLevels.Length)].Algorithm);
            }
        }


        public void StartNextRound(
            int labyrinthId,
            int algorithmId)
        {
            StartNextRound(
                Promoscience.Labyrinths.Resources.Instance.GetLabyrinth(labyrinthId), 
                (Algorithms.Id)algorithmId);
        }

        public void StartNextRound(
            IData labyrinth,
            Algorithms.Id algorithmId)
        {
            Round.Value = (Round.Value + 1).Mod(Server.Instance.Settings.NumberOfRounds.Value);

            CurrentLabyrinth = labyrinth;

            baseAlgorithmId = algorithmId;

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

                        Debug.Log(player);

                        player.serverAlgorithm = baseAlgorithmId;

                        player.serverLabyrinthId = CurrentLabyrinth.Id;

                        AssignCourse(player);

                        player.TargetSetGame(
                            player.connectionToClient,
                            CurrentLabyrinth.Json,
                            player.serverAlgorithm,
                            Round.Value);

                        break;
                }
            }

            DoStartRound();
        }


        protected virtual void DoStartRound()
        {
            Labyrinths.Add(CurrentLabyrinth);

            roundState = ServerState.Round;

            Server.Instance.State.Set(ServerState.Round);
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
                Round.Value);
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

        //public void EndRoundOrTutorial()
        //{
        //    for (int i = 0; i < PlayerList.instance.list.Count; i++)
        //    {
        //        Player player = PlayerList.instance.GetPlayerWithId(i);
        //        SQLiteUtilities.SetCourseInactive(player.ServerCourseId);

        //        if (player.ServerPlayerGameState == ClientGameState.PlayingTutorial ||
        //            player.ServerPlayerGameState == ClientGameState.Playing)
        //        {
        //            player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextRound);
        //            player.TargetSetEndRoundOrTutorial(player.connectionToClient);
        //        }
        //    }
        //}

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