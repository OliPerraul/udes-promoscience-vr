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
        private LevelPreset[] predefinedLevels;

        private List<Level> levels = new List<Level>();

        public IList<Level> Levels => levels;

        private Level currentLevel;

        public Level CurrentLevel => currentLevel;

        [SerializeField]
        protected LevelSelectionMode levelSelectionMode;

        [SerializeField]
        private const int tutorialLabyrinthId = 4;

        [SerializeField]
        private Algorithms.Id baseAlgorithmId;

        public virtual ServerState LevelState => ServerState.Level;

        // TODO replace listening to server state with this
        //public Cirrus.Event<Round> OnRoundStartedHandler;

        //public Cirrus.Event<Round> OnRoundEndedHandler;

        //public List<Course> Courses = new List<Course>();

        private int NextLevelNumber => currentLevel == null ? 0 : (currentLevel.Number + 1).Mod(Server.Instance.Settings.NumberOfRounds.Value);

        public Game(
            int id)
        {
            this.Id = id;

            levelSelectionMode = LevelSelectionMode.Selected;
        }

        public Game(
            int id,
            LevelPreset[] predefinedLevels)
        {
            this.predefinedLevels = predefinedLevels;

            this.Id = id;

            levelSelectionMode = LevelSelectionMode.Predefined;
        }

        public void Start()
        {
            StartNextLevel();            
        }

        // Try find course ID initiated by a team member
        // Otherwise assign new course
        //
        // Returns true if created a course

        // Assumes ServerTeamId is set
        // BUG
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
                    Labyrinth = currentLevel.Labyrinth,
                    Algorithm = Algorithms.Resources.Instance.GetAlgorithm(player.serverAlgorithm),
                    //AlgorithmSteps = algorithm.GetAlgorithmSteps(currentRound.Labyrinth) // labyrinth)  
                };

                currentLevel.Courses.Add(course);
                player.ServerCourseId = course.Id;

                SQLiteUtilities.InsertPlayerCourse(
                    player.ServerTeamId,
                    player.serverLabyrinthId,
                    (int)player.serverAlgorithm,
                    player.ServerCourseId,
                    player.ServerLevelNumber,
                    Id);
            }
        }

        public void Stop()
        {

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
                    player.TargetSetGameState(player.connectionToClient, ClientGameState.WaitingForNextLevel);
                }
            }

            Server.Instance.State.Set(ServerState.ThanksForPlaying);
        }

        public void StartNextLevel()
        {
            if (levelSelectionMode == LevelSelectionMode.Selected)
            {
                Server.Instance.State.Set(ServerState.LevelSelect);
            }
            else
            {

                StartNextLevel(
                    predefinedLevels[NextLevelNumber.Mod(predefinedLevels.Length)].Labyrinth,
                    predefinedLevels[NextLevelNumber.Mod(predefinedLevels.Length)].Algorithm);
            }
        }


        public void StartNextLevel(
            int labyrinthId,
            int algorithmId)
        {
            StartNextLevel(
                LabyrinthManager.Instance.GetLabyrinth(labyrinthId), 
                (Algorithms.Id)algorithmId);
        }

        public void StartNextLevel(
            ILabyrinth labyrinth,
            Algorithms.Id algorithmId)
        {
            currentLevel = new Level
            {
                Number = NextLevelNumber,
                Labyrinth = labyrinth,
                Algorithm = Algorithms.Resources.Instance.GetAlgorithm(algorithmId)
            };

            levels.Add(currentLevel);

            SQLiteUtilities.InsertLevel(
                SQLiteUtilities.GetNextLevelID(),
                currentLevel.Number,
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
                    case ClientGameState.WaitingForNextLevel:

                        Debug.Log(player);

                        player.serverAlgorithm = baseAlgorithmId;

                        player.serverLabyrinthId = currentLevel.Labyrinth.Id;

                        AssignCourse(player);

                        player.TargetSetGame(
                            player.connectionToClient,
                            currentLevel.Labyrinth.Json,
                            player.serverAlgorithm,
                            currentLevel.Number);

                        break;
                }
            }

            Server.Instance.State.Set(LevelState);
        }



        public void JoinGameLevel(Player player)
        {
            AssignCourse(player);

            player.serverAlgorithm = Algorithms.Utils.GetLevelAlgorithm((int)baseAlgorithmId, player.ServerTeamId);

            player.serverLabyrinthId = currentLevel.Labyrinth.Id;

            player.TargetSetGame(
                player.connectionToClient,
                currentLevel.Labyrinth.Json,
                player.serverAlgorithm,
                currentLevel.Number);
        }

        public void JoinGameLevelWithSteps(Player player, int[] steps)
        {
            AssignCourse(player);

            player.serverAlgorithm = Algorithms.Utils.GetLevelAlgorithm((int)baseAlgorithmId, player.ServerTeamId);

            player.serverLabyrinthId = currentLevel.Labyrinth.Id;

            player.TargetSetGameWithSteps(
                player.connectionToClient,
                steps,
                currentLevel.Labyrinth.Json,
                player.serverAlgorithm,
                currentLevel.Number,
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