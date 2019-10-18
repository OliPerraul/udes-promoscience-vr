using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay.UI
{
    public class Replay : MonoBehaviour
    {
        [SerializeField]
        private ScriptableReplayOptions replayOptions;

        [SerializeField]
        private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private UnityEngine.UI.Button openButton;

        [SerializeField]
        private UnityEngine.UI.Button exitButton;

        [SerializeField]
        private UnityEngine.UI.Button overlayButton;

        [SerializeField]
        private UnityEngine.UI.Button algorithmButton;

        [SerializeField]
        private GameObject sequenceToggle;

        [SerializeField]
        private SequencePopup SequencePopup;

        [SerializeField]
        private GameObject overlay;

        private bool init = false;

        public void OnEnable()
        {
            if (init)
                return;

            init = true;

            sequenceToggle.SetActive(false);
            SequencePopup.gameObject.SetActive(false);

            openButton.onClick.AddListener(OnOpenClicked);
            exitButton.onClick.AddListener(OnExitClicked);
            overlayButton.onClick.AddListener(OnOverlayClicked);
            algorithmButton.onClick.AddListener(OnAlgorithmClicked);

            server.gameStateChangedEvent += OnGameStateChanged;
        }


        public void OnGameStateChanged()
        {
            switch (server.GameState)
            {
                case Promoscience.Utils.ServerGameState.IntermissionReplay:
                case Promoscience.Utils.ServerGameState.FinalReplay:
                    Enabled = true;
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }





        public void OnAlgorithmClicked()
        {
            replayOptions.SendAction(ReplayAction.ToggleAlgorithm);
        }

        private void OnOverlayClicked()
        {
            overlay.SetActive(!overlay.activeSelf);
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
                SequencePopup.gameObject.SetActive(_enabled);

            }
        }

        public void OnOpenClicked()
        {
            SequencePopup.gameObject.SetActive(!sequenceToggle.activeInHierarchy);
            sequenceToggle.SetActive(!sequenceToggle.activeInHierarchy);
            overlayButton.gameObject.SetActive(!overlayButton.gameObject.activeInHierarchy);
            algorithmButton.gameObject.SetActive(!algorithmButton.gameObject.activeInHierarchy);
        }

        public void OnExitClicked()
        {
            server.EndRoundOrTutorial();
        }

    }
}