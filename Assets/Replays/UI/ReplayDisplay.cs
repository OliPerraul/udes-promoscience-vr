using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplayDisplay : MonoBehaviour
    {
        [SerializeField]
        protected ControllerAsset replayOptions;

        [SerializeField]
        private UnityEngine.UI.Text algorithmNameText;

        [SerializeField]
        private UnityEngine.UI.Text algorithmStepsText;

        [SerializeField]
        private LocalizeInlineString algorithmStepsString = new LocalizeInlineString("Number of steps: ");

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
            replayOptions.OnMoveIndexChanged += OnMoveIndexChanged;

            Server.Instance.gameStateChangedEvent += OnGameStateChanged;

            Enabled = false;
        }

        public void OnMoveIndexChanged(int idx)
        {
            if(course != null)
            algorithmStepsText.text = algorithmStepsString.Value + course.CurrentAlgorithmMoveIndex;
        }

        public void OnGreyboxClicked()
        {
            replayOptions.SendAction(ReplayAction.ToggleGreyboxLabyrinth);
        }


        private Course course = null;

        public void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ToggleOptions:
                    bool enable = (bool)args[0];
                    EnableOptions(enable);
                    break;

                case ReplayAction.ToggleDirtyLabyrinth:
                    if (args.Length == 0)
                        EnableAlgorithm(!isAlgorithmEnabled);
                    else
                        EnableAlgorithm((bool)args[0]);

                    break;

                case ReplayAction.SequenceSelected:
                    course = (Course)args[0];

                    algorithmNameText.text = course.Algorithm.Name;
                    algorithmStepsText.text = algorithmStepsString.Value + course.CurrentAlgorithmMoveIndex;
                    break;
            }
        }


        public void OnGameStateChanged()
        {
            switch (Server.Instance.GameState)
            {
                case ServerGameState.AdvancedReplay:
                case ServerGameState.InstantReplay:
                    Enabled = true;
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
                greyboxButton.gameObject.SetActive(_enabled);
                sequenceToggle.gameObject.SetActive(_enabled);
                sequencePopup.gameObject.SetActive(_enabled);
                controls.gameObject.SetActive(_enabled);
                algorithmNameText.gameObject.SetActive(_enabled);
                algorithmStepsText.gameObject.SetActive(_enabled);

            }
        }

        public void EnableOptions(bool enable)
        {
            sequencePopup.gameObject.SetActive(enable);
            sequenceToggle.SetActive(enable);
            overlayButton.gameObject.SetActive(enable);
            algorithmButton.gameObject.SetActive(enable);
            greyboxButton.gameObject.SetActive(enable);
        }

        bool isAlgorithmEnabled = true;

        public void EnableAlgorithm(bool enable)
        {
            isAlgorithmEnabled = enable;
            algorithmNameText.gameObject.SetActive(enable);
            algorithmStepsText.gameObject.SetActive(enable);
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