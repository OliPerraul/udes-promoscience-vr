using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdeS.Promoscience.UI;

namespace UdeS.Promoscience.Network.UI
{
    // Controls which UI feature are enabled for given state of the server

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

        // End Thx for playing screen
        End = 1 << 8,
    }

    public class LobbyControls : MonoBehaviour
    {
        public Cirrus.ObservableValue<LobbyControlsFlag> Flags = new Cirrus.ObservableValue<LobbyControlsFlag>();

        [SerializeField]
        private GameObject header;

        [SerializeField]
        private GameObject body;

        [SerializeField]
        private GameObject end;

        [SerializeField]
        private GameObject bottom;

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

        [SerializeField]
        private UnityEngine.UI.Button endThanksButton;

        private Game game;

        private Round round;


        public bool Enabled { set => target?.gameObject?.SetActive(value); }

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerGameStateChanged;
            GameManager.Instance.OnGameStartedHandler += OnGameStarted;

            Flags.OnValueChangedHandler += OnFlagsChanged;
            quickPlayButton.onClick.AddListener(() => Server.Instance.StartQuickplay());
            newGameButton.onClick.AddListener(() => Server.Instance.StartGame());
            instantReplayButton.onClick.AddListener(() => Server.Instance.StartInstantReplay());
            advancedReplayButton.onClick.AddListener(() => Server.Instance.StartAdvancedReplay());
            nextRoundButton.onClick.AddListener(() => Server.Instance.StartNextRound());
            endGameButton.onClick.AddListener(() => Server.Instance.StopGame());
            endThanksButton.onClick.AddListener(() => Server.Instance.ReturnToLobby());
            
        }

        public void Start()
        {
            OnServerGameStateChanged(Server.Instance.State.Value);
        }

        public void OnDestroy()
        {
            if (Server.Instance != null && Server.Instance.gameObject != null) Server.Instance.State.OnValueChangedHandler -= OnServerGameStateChanged;
        }

        public void OnGameStarted(Game game)
        {
            game.OnRoundStartedHandler += OnRoundStarted;
        }


        public void OnRoundStarted(Round round)
        {
            this.round = round;
        }

        public void OnServerGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.Quickplay:
                    Flags.Value =
                        LobbyControlsFlag.InstantReplay |
                        LobbyControlsFlag.EndGame;
                    break;

                case ServerState.Round:
                    Flags.Value =
                        LobbyControlsFlag.InstantReplay |
                        LobbyControlsFlag.AdvancedReplay |
                        LobbyControlsFlag.NextRound |
                        LobbyControlsFlag.EndGame;

                    // If first round do not allow advanced replay feature
                    if (round.Number < 1)
                    {
                        Flags.Value = Flags.Value & ~LobbyControlsFlag.AdvancedReplay;
                    }

                    

                    // If last round only allow to end the game
                    if (Server.Instance.Settings.NumberOfRounds.Value - 1 ==
                        round.Number)
                    {
                        Flags.Value = Flags.Value & ~LobbyControlsFlag.NextRound;
                    }

                    break;

                case ServerState.Lobby:
                    Flags.Value =
                        LobbyControlsFlag.QuickPlay |
                        LobbyControlsFlag.NewGame;//| ServerControlsFlag;
                    break;

                case ServerState.ThanksForPlaying:
                    Flags.Value =
                        LobbyControlsFlag.End;//| ServerControlsFlag;
                    break;
            }

            switch (state)
            {
                case ServerState.Round:
                case ServerState.Quickplay:
                    body.SetActive(false);
                    bottom.SetActive(true);
                    end.SetActive(false);
                    break;

                case ServerState.LevelSelect:
                case ServerState.ReplaySelect:
                case ServerState.LabyrinthReplay:
                    body.SetActive(false);
                    bottom.SetActive(false);
                    end.SetActive(false);
                    break;

                case ServerState.ThanksForPlaying:
                    body.SetActive(false);
                    bottom.SetActive(true);
                    end.SetActive(true);
                    break;

                case ServerState.Menu:
                    break;

                default:
                    body.SetActive(true);
                    bottom.SetActive(true);
                    end.SetActive(false);
                    break;
            }
        }

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

            endThanksButton?
                .gameObject.SetActive((flags & LobbyControlsFlag.End) != 0);
            //TODO
            //restartRoundButton?
            //    .gameObject.SetActive((flags & ServerControlsFlag.RestartRound) != 0);
        }

    }
}