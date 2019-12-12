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
        private UI.SidebarAsset roundSidebar;

        [SerializeField]
        private UI.SelectedTeamAsset roundSelectedTeam;

        [SerializeField]
        private TeamToggleAsset roundTeamToggle;

        [SerializeField]
        private ReplayControlsAsset roundReplayControls;

        [SerializeField]
        private AlgorithmSelectionAsset roundAlgorithmSelection;

        public Cirrus.ObservableValue<LevelReplay> LevelReplay = new Cirrus.ObservableValue<LevelReplay>();


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
                GameManager.Instance.CurrentGame.Levels);

            GameReplay.Value.Initialize();

            GameReplay.Value.Start();

        }


        public void StartCurrentLevelReplay()
        {
            StartLevelReplay(GameManager.Instance.CurrentGame.CurrentLevel);   
        }


        public void StartLevelReplay(Level round)
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

            LevelReplay.Value = new LevelReplay(
                roundReplayControls,
                roundAlgorithmSelection,
                roundTeamToggle,
                roundSelectedTeam, 
                roundSidebar,
                round);

            LevelReplay.Value.Initialize();

            LevelReplay.Value.Start();
        }



        public void OnServerStateValueChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.GameReplay:

                    if (LevelReplay.Value != null)
                    {
                        LevelReplay.Value.Clear();
                        LevelReplay.Value = null;
                    }

                    break;

                case ServerState.LevelReplay:
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

                    if (LevelReplay.Value != null)
                    {
                        LevelReplay.Value.Clear();
                        LevelReplay.Value = null;
                    }

                    break;
            }

        }
    }
}