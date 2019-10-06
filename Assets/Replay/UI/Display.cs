using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay.UI
{
    public class Display : MonoBehaviour
    {
        [SerializeField]
        private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private UnityEngine.UI.Button openButton;

         [SerializeField]
        private UnityEngine.UI.Button exitButton;

        [SerializeField]
        private GameObject sequenceToggle;

        [SerializeField]
        private SequencePopup SequencePopup;

        public void Awake()
        {
            sequenceToggle.SetActive(false);
            SequencePopup.gameObject.SetActive(false);

            openButton.onClick.AddListener(OnOpenClicked);
            exitButton.onClick.AddListener(OnExitClicked);
            server.gameStateChangedEvent += OnGameStateChanged;

            //replayO

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
                sequenceToggle.gameObject.SetActive(_enabled);
            }
        }

        public void OnOpenClicked()
        {
            SequencePopup.gameObject.SetActive(!sequenceToggle.activeInHierarchy);
            sequenceToggle.SetActive(!sequenceToggle.activeInHierarchy);
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