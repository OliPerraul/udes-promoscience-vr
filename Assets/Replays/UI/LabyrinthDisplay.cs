using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replays.UI
{
    public class LabyrinthDisplay : MonoBehaviour
    {
        [SerializeField]
        protected ScriptableController replayOptions;

        //[SerializeField]
        //protected ServerGametion server;

        [SerializeField]
        protected Controls controls;

        [SerializeField]
        protected UnityEngine.UI.Button openButton;

        [SerializeField]
        protected UnityEngine.UI.Button exitButton;

        [SerializeField]
        protected UnityEngine.UI.Button overlayButton;

        [SerializeField]
        protected UnityEngine.UI.Button greyboxButton;

        [SerializeField]
        protected UnityEngine.UI.Button algorithmButton;

        [SerializeField]
        protected GameObject sequenceToggle;

        [SerializeField]
        protected SequencePopup sequencePopup;

        [SerializeField]
        private GameObject overlay;

        protected bool init = false;

        public virtual void OnEnable()
        {
            if (init)
                return;

            init = true;

            openButton.onClick.AddListener(OnOpenClicked);
            exitButton.onClick.AddListener(OnExitClicked);
            overlayButton.onClick.AddListener(OnOverlayClicked);
            algorithmButton.onClick.AddListener(OnAlgorithmClicked);
            greyboxButton.onClick.AddListener(OnGreyboxClicked);

            replayOptions.OnActionHandler += OnReplayAction;

            Server.Instance.gameStateChangedEvent += OnGameStateChanged;

            Enabled = false;
        }

        public void OnGreyboxClicked()
        {
            replayOptions.SendAction(ReplayAction.ToggleGreyboxLabyrinth);
        }


        public void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ToggleOptions:
                    bool enable = (bool)args[0];
                    EnableOptions(enable);
                    break;
            }
        }


        [SerializeField]
        private bool isInstantReplay = false;

        public void OnGameStateChanged()
        {
            switch (Server.Instance.GameState)
            {
                case ServerGameState.AdvancedReplay:                    
                    Enabled = !isInstantReplay;
                    break;

                case ServerGameState.InstantReplay:
                    Enabled = isInstantReplay;
                    break;

                default:
                    Enabled = false;
                    break;
            }
        }


        public void OnAlgorithmClicked()
        {
            replayOptions.SendAction(ReplayAction.ToggleDirtyLabyrinth);
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
                gameObject.SetActive(_enabled);
                openButton.gameObject.SetActive(_enabled);
                exitButton.gameObject.SetActive(_enabled);
                sequenceToggle.gameObject.SetActive(_enabled);
                sequencePopup.gameObject.SetActive(_enabled);
                controls.gameObject.SetActive(_enabled);

            }
        }

        public void EnableOptions(bool enable)
        {
            sequencePopup.gameObject.SetActive(enable);
            sequenceToggle.SetActive(enable);
            overlayButton.gameObject.SetActive(enable);
            algorithmButton.gameObject.SetActive(enable);
        }

        public void OnOpenClicked()
        {
            EnableOptions(!sequenceToggle.activeInHierarchy);
        }

        public void OnExitClicked()
        {
            //server.EndRoundOrTutorial();
            Enabled = false;
            replayOptions.SendAction(ReplayAction.ExitReplay);
        }

    }
}