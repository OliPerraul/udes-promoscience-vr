using UnityEngine;
using System.Collections;
using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using UdeS.Promoscience.UI;
using System.Collections.Generic;

namespace UdeS.Promoscience.Replays
{
    public class ReplayManager : Cirrus.BaseSingleton<ReplayManager>
    {
        //private BaseReplay CurrentReplay;

        //public SplitReplay SplitReplay;

        public Cirrus.Event<LabyrinthReplay> OnLabyrinthReplayStartedHandler;

        public Cirrus.Event<SplitReplay> OnSplitReplayStartedHandler;

        private Game currentGame;

        private Round currentRound;

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnGameStateValueChanged;
            GameManager.Instance.OnGameStartedHandler += OnGameStarted;
            GameManager.Instance.OnGameEndedHandler += OnGameEnded;
        }

        

        public void OnGameStarted(Game game)
        {
            currentGame = game;

            currentGame.OnRoundEndedHandler += OnRoundStarted;

            currentGame.OnRoundEndedHandler += OnRoundEnded;
        }

        public void OnGameEnded(Game game)
        {
            currentGame = game;
        }

        public void OnRoundStarted(Round round)
        {
            currentRound = round;
        }

        public void OnRoundEnded(Round round)
        {
            currentRound = round;
        }



        public void StartReplaySelect()
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

            List<Round> rounds = new List<Round>();

            rounds.AddRange(currentGame.Rounds);

            var replay = new SplitReplay(
                currentGame.Rounds);

            replay.Start();

            OnSplitReplayStartedHandler?.Invoke(replay);
        }


        public void OnGameStateValueChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LabyrinthReplay:
                case ServerState.ReplaySelect:
                    break;

                default:
                    //if (CurrentReplay != null)
                    //{
                    //    CurrentReplay.Clear();
                    //    CurrentReplay = null;
                    //}

                    break;
            }

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

            var replay = new InstantReplay(
                SQLiteUtilities.GetCoursesForGameRound(
                    currentGame.Id,
                    currentRound.Number),
                currentRound.Labyrinth);

            replay.Start();
            OnLabyrinthReplayStartedHandler?.Invoke(replay);
        }

        public void Update()
        {

        }
    }
}