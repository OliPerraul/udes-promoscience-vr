using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay.UI
{
    public class Menu : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private UnityEngine.UI.Button openButton;

         [SerializeField]
        private UnityEngine.UI.Button exitButton;

        [SerializeField]
        private Controls display;


        public void Awake()
        {
            display.gameObject.SetActive(true);
            display.Enabled = false;

            openButton.onClick.AddListener(OnOpenClicked);
            exitButton.onClick.AddListener(OnExitClicked);
            server.gameStateChangedEvent += OnGameStateChanged;
        }

        private bool _enabled = false;

        public bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                _enabled = value;
                openButton.gameObject.SetActive(_enabled);
                exitButton.gameObject.SetActive(_enabled);
                display.gameObject.SetActive(_enabled);
            }
        }

        public void OnOpenClicked()
        {
            display.Enabled = !display.Enabled;//SetActive(!display.gameObject.activeInHierarchy);
        }

        public void OnExitClicked()
        {
            server.EndRoundOrTutorial();
        }

        public void OnGameStateChanged()
        {
            switch (server.GameState)
            {
                case Utils.ServerGameState.ViewingPlayback:
                    Enabled = true;
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }

    }
}