﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Network.UI
{

    [System.Serializable]
    public enum ServerControlsFlag// : int
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

    public class ServerControls : MonoBehaviour
    {
        public Cirrus.ObservableValue<ServerControlsFlag> Flags = new Cirrus.ObservableValue<ServerControlsFlag>();

        public void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnServerGameStateChanged;
            Flags.OnValueChangedHandler += OnFlagsChanged;
        }

        public bool Enabled
        {
            set
            {
                target?.gameObject?.SetActive(value);
            }
        }

        public void OnServerGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.Round:
                case ServerState.Lobby:
                case ServerState.Tutorial:
                case ServerState.Intermission:
                    Enabled = true;

                    switch (state)
                    {
                        case ServerState.Quickplay:
                            Flags.Value =
                                ServerControlsFlag.InstantReplay |
                                ServerControlsFlag.NewGame |
                                ServerControlsFlag.RestartRound |
                                ServerControlsFlag.EndGame;
                            break;

                        case ServerState.Round:
                            Flags.Value =
                               ServerControlsFlag.InstantReplay |
                               ServerControlsFlag.AdvancedReplay;
                            break;


                    }

                    break;

                default:
                    Enabled = false;
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

        public void OnFlagsChanged(ServerControlsFlag flags)
        {
            advancedReplayButton?
                .gameObject.SetActive((flags & ServerControlsFlag.AdvancedReplay) != 0);
            endGameButton?
                .gameObject.SetActive((flags & ServerControlsFlag.EndGame) != 0);
            endRoundButton?
                .gameObject.SetActive((flags & ServerControlsFlag.EndRound) != 0);
            instantReplayButton?
                .gameObject.SetActive((flags & ServerControlsFlag.InstantReplay) != 0);
            newGameButton?
                .gameObject.SetActive((flags & ServerControlsFlag.NewGame) != 0);
            nextRoundButton?
                .gameObject.SetActive((flags & ServerControlsFlag.NextRound) != 0);
            quickPlayButton?
                .gameObject.SetActive((flags & ServerControlsFlag.QuickPlay) != 0);
            restartRoundButton?
                .gameObject.SetActive((flags & ServerControlsFlag.RestartRound) != 0);
        }

    }
}