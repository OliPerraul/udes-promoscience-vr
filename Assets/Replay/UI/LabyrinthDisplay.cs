using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replay.UI
{
    public class LabyrinthDisplay : MonoBehaviour
    {
        [SerializeField]
        protected ScriptableReplayController replayOptions;

        [SerializeField]
        protected ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        protected UnityEngine.UI.Button openButton;

        [SerializeField]
        protected UnityEngine.UI.Button exitButton;

        [SerializeField]
        protected UnityEngine.UI.Button overlayButton;

        [SerializeField]
        protected UnityEngine.UI.Button algorithmButton;

        [SerializeField]
        protected GameObject sequenceToggle;

        [SerializeField]
        protected SequencePopup SequencePopup;

        [SerializeField]
        private GameObject overlay;

        protected bool init = false;

        public virtual void OnEnable()
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
            //switch (server.GameState)
            //{
            //    case Promoscience.Utils.ServerGameState.IntermissionReplay:
            //    case Promoscience.Utils.ServerGameState.FinalReplay:
            //        Enabled = true;
            //        break;

            //    default:
            //        Enabled = false;
            //        break;
            //}
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

        public virtual bool Enabled
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