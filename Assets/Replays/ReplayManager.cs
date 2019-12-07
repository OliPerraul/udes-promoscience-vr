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
        [SerializeField]
        private ReplayControlsAsset roundReplayControls;

        public Cirrus.ObservableValue<RoundReplay> RoundReplay = new Cirrus.ObservableValue<RoundReplay>();


        [SerializeField]
        private ReplayControlsAsset gameReplayControls;

        public Cirrus.ObservableValue<GameReplay> GameReplay = new Cirrus.ObservableValue<GameReplay>();


        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerStateValueChanged;
            //GameManager.Instance.OnGameCreatedHandler += OnGameCreated;
            //GameManager.Instance.OnGameEndedHandler += OnGameEnded;
        }

        public void Update()
        {

        }

        public void StartGameReplay()
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

            GameReplay.Value = new GameReplay(
                gameReplayControls,
                GameManager.Instance.CurrentGame.Rounds);

            GameReplay.Value.Start();
        }

        public void StartRoundReplay()
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

            RoundReplay.Value = new InstantReplay(
                roundReplayControls,
                GameManager.Instance.CurrentGame.CurrentRound);

            RoundReplay.Value.Start();
        }


        public void OnServerStateValueChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.GameReplay:

                    if (RoundReplay.Value != null)
                    {
                        RoundReplay.Value.Clear();
                        RoundReplay.Value = null;
                    }

                    break;

                case ServerState.RoundReplay:
                    if (GameReplay.Value != null)
                    {
                        GameReplay.Value.Clear();
                        GameReplay.Value = null;
                    }
                    break;

                default:

                    if (GameReplay.Value != null)
                    {
                        GameReplay.Value.Clear();
                        GameReplay.Value = null;
                    }

                    if (RoundReplay.Value != null)
                    {
                        RoundReplay.Value.Clear();
                        RoundReplay.Value = null;
                    }

                    break;
            }

        }
    }
}