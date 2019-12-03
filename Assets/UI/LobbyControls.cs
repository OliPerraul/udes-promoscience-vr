using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdeS.Promoscience.UI;

namespace UdeS.Promoscience.Network.UI
{

    [System.Serializable]
    public enum LobbyControlsFlag// : int
    {
        None = 0,//1 << 0,
        QuickPlay = 1 << 0,
        NewGame = 1 << 1,
        EndGame = 1 << 2,
        NextRound = 1 << 3,
        RestartRound = 1 << 4,
        EndRound = 1 << 5,
        InstantReplay = 1 << 6,
        AdvancedReplay = 1 << 7,
    }

    public class LobbyControls : MonoBehaviour
    {
        public Cirrus.ObservableValue<LobbyControlsFlag> Flags = new Cirrus.ObservableValue<LobbyControlsFlag>();

        [SerializeField]
        private GameObject header;

        [SerializeField]
        private GameObject body;

        [SerializeField]
        private GameObject bottom;


        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerGameStateChanged;
            ButtonManager.Instance.ExitButton.onClick.AddListener(OnExitClicked);
            
            Flags.OnValueChangedHandler += OnFlagsChanged;
            quickPlayButton.onClick.AddListener(() => Server.Instance.StartQuickplay());
            newGameButton.onClick.AddListener(() => Server.Instance.StartGame());
            instantReplayButton.onClick.AddListener(() => Server.Instance.StartInstantReplay());
        }

        public void OnExitClicked()
        {
            switch (Server.Instance.State.Value)
            {
                case ServerState.Lobby:
                case ServerState.Quickplay:
                case ServerState.Round:

                    break;


                case ServerState.LevelSelect:
                case ServerState.ReplaySelect:

                    break;
            }
        }


        public bool Enabled { set => target?.gameObject?.SetActive(value); }

        public void OnServerGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.Quickplay:
                    Flags.Value =
                        LobbyControlsFlag.InstantReplay |
                        LobbyControlsFlag.RestartRound |
                        LobbyControlsFlag.EndGame;
                    break;

                case ServerState.Round:
                    Flags.Value =
                        LobbyControlsFlag.InstantReplay |
                        LobbyControlsFlag.AdvancedReplay;
                    break;

                case ServerState.Lobby:
                    Flags.Value =
                        LobbyControlsFlag.QuickPlay |
                        LobbyControlsFlag.NewGame;//| ServerControlsFlag;
                    break;
            }

            switch (state)
            {
                case ServerState.Round:
                case ServerState.Quickplay:
                    body.SetActive(false);
                    bottom.SetActive(true);

                    break;

                case ServerState.LevelSelect:
                case ServerState.ReplaySelect:
                case ServerState.LabyrinthReplay:
                    body.SetActive(false);
                    bottom.SetActive(false);
                    break;


                default:
                    body.SetActive(true);
                    bottom.SetActive(true);
                    break;

            }
        }


        [SerializeField]
        private GameObject target;

        [SerializeField]
        private UnityEngine.UI.Button quickPlayButton;

        [SerializeField]
        private UnityEngine.UI.Button newGameButton;

        [SerializeField]
        private UnityEngine.UI.Button endGameButton;

        [SerializeField]
        private UnityEngine.UI.Button nextRoundButton;

        [SerializeField]
        private UnityEngine.UI.Button restartRoundButton;

        [SerializeField]
        private UnityEngine.UI.Button endRoundButton;

        [SerializeField]
        private UnityEngine.UI.Button instantReplayButton;

        [SerializeField]
        private UnityEngine.UI.Button advancedReplayButton;

        public void OnFlagsChanged(LobbyControlsFlag flags)
        {
            advancedReplayButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.AdvancedReplay) != 0);
            endGameButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.EndGame) != 0);
            endRoundButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.EndRound) != 0);
            instantReplayButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.InstantReplay) != 0);
            newGameButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.NewGame) != 0);
            nextRoundButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.NextRound) != 0);
            quickPlayButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.QuickPlay) != 0);
            //TODO
            //restartRoundButton?
            //    .gameObject.SetActive((flags & ServerControlsFlag.RestartRound) != 0);
        }

    }
}