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
        public int Id = 0;

        [SerializeField]
        private RoundPreset[] predefinedLevels;

        private List<Round> rounds = new List<Round>();

        public IList<Round> Rounds => rounds;

        private Round currentRound;

        public Round CurrentRound => currentRound;

        [SerializeField]
        protected LevelSelectionMode levelSelectionMode;

        [SerializeField]
        private const int tutorialLabyrinthId = 4;

        [SerializeField]
        private Algorithms.Id baseAlgorithmId;

        public virtual ServerState RoundState => ServerState.Round;

        // TODO replace listening to server state with this
        //public Cirrus.Event<Round> OnRoundStartedHandler;

        //public Cirrus.Event<Round> OnRoundEndedHandler;

        public List<Course> Courses = new List<Course>();

        private int NextRoundNumber => currentRound == null ? 0 : (currentRound.Number + 1).Mod(Server.Instance.Settings.NumberOfRounds.Value);

        public Game(
            int id)
        {
            this.Id = id;

            levelSelectionMode = LevelSelectionMode.Selected;
        }

        public Game(
            int id,
            RoundPreset[] predefinedLevels)
        {
            this.predefinedLevels = predefinedLevels;

            this.Id = id;

            levelSelectionMode = LevelSelectionMode.Predefined;
        }

        public void Start()
        {
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
                course = new Course
                {
                    Id = SQLiteUtilities.GetNextCourseID(),
                    Team = Teams.Resources.Instance.GetScriptableTeamWithId(player.ServerTeamId),
                    Labyrinth = currentRound.Labyrinth,
                    Algorithm = Algorithms.Resources.Instance.GetAlgorithm(player.serverAlgorithm),
                    //AlgorithmSteps = algorithm.GetAlgorithmSteps(currentRound.Labyrinth) // labyrinth)  
                };

                Courses.Add(course);
                player.ServerCourseId = course.Id;

                SQLiteUtilities.InsertPlayerCourse(
                    player.ServerTeamId,
                    player.serverLabyrinthId,
                    (int)player.serverAlgorithm,
                    player.ServerCourseId,
                    player.ServerRoundNumber,
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
                    predefinedLevels[NextRoundNumber.Mod(predefinedLevels.Length)].Labyrinth,
                    predefinedLevels[NextRoundNumber.Mod(predefinedLevels.Length)].Algorithm);
            }
        }


        public void StartNextRound(
            int labyrinthId,
            int algorithmId)
        {
            StartNextRound(
                Labyrinths.Resources.Instance.GetLabyrinth(labyrinthId), 
                (Algorithms.Id)algorithmId);
        }

        public void StartNextRound(
            ILabyrinth labyrinth,
            Algorithms.Id algorithmId)
        {
            currentRound = new Round
            {
                Number = NextRoundNumber,
                Labyrinth = labyrinth,
                Algorithm = Algorithms.Resources.Instance.GetAlgorithm(algorithmId)
            };

            SQLiteUtilities.InsertRound(
                SQLiteUtilities.GetNextRoundID(),
                currentRound.Number,
                Id,
                labyrinth.Id
                );

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

                        player.serverLabyrinthId = currentRound.Labyrinth.Id;

                        AssignCourse(player);

                        player.TargetSetGame(
                            player.connectionToClient,
                            currentRound.Labyrinth.Json,
                            player.serverAlgorithm,
                            currentRound.Number);

                        break;
                }
            }

            Server.Instance.State.Set(RoundState);
        }



        public void JoinGameRound(Player player)
        {
            AssignCourse(player);

            player.serverAlgorithm = Algorithms.Utils.GetRoundAlgorithm((int)baseAlgorithmId, player.ServerTeamId);

            player.serverLabyrinthId = currentRound.Labyrinth.Id;

            player.TargetSetGame(
                player.connectionToClient,
                currentRound.Labyrinth.Json,
                player.serverAlgorithm,
                currentRound.Number);
        }

        public void JoinGameRoundWithSteps(Player player, int[] steps)
        {
            AssignCourse(player);

            player.serverAlgorithm = Algorithms.Utils.GetRoundAlgorithm((int)baseAlgorithmId, player.ServerTeamId);

            player.serverLabyrinthId = currentRound.Labyrinth.Id;

            player.TargetSetGameWithSteps(
                player.connectionToClient,
                steps,
                currentRound.Labyrinth.Json,
                player.serverAlgorithm,
                currentRound.Number,
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