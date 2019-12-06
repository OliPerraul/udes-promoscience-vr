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

        public Cirrus.Event<LabyrinthReplay> OnLabyrinthReplayCreatedHandler;

        public Cirrus.Event<SplitReplay> OnSplitReplayCreatedHandler;

        private Game currentGame;

        private Round currentRound;

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnGameStateValueChanged;
            GameManager.Instance.OnGameCreatedHandler += OnGameCreated;
            GameManager.Instance.OnGameEndedHandler += OnGameEnded;
        }

        

        public void OnGameCreated(Game game)
        {
            currentGame = game;

            currentGame.OnRoundStartedHandler += OnRoundStarted;
        }

        public void OnGameEnded(Game game)
        {
            currentGame = game;
        }

        public void OnRoundStarted(Round round)
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

            var replay = new SplitReplay(
                currentGame.Rounds);

            OnSplitReplayCreatedHandler?.Invoke(replay);
            replay.Start();
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

            OnLabyrinthReplayCreatedHandler?.Invoke(replay);
            replay.Start();
        }

        public void Update()
        {

        }
    }
}