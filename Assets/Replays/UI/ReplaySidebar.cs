using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdeS.Promoscience.UI;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySidebar : MonoBehaviour
    {
        [SerializeField]
        protected ReplayManagerAsset replayOptions;

        [SerializeField]
        private UnityEngine.UI.Text algorithmNameText;

        [SerializeField]
        private UnityEngine.UI.Text algorithmStepsText;

        [SerializeField]
        private LocalizeInlineString algorithmStepsString = new LocalizeInlineString("Number of steps: ");

        [SerializeField]
        protected Controls controls;

        [SerializeField]
        [UnityEngine.Serialization.FormerlySerializedAs("openButton")]
        protected UnityEngine.UI.Button infoButton;

        [SerializeField]
        protected UnityEngine.UI.Button closeButton;

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

        public virtual void Awake()
        {
            ButtonManager.Instance.InfoButton.onClick.AddListener(() => Enabled = !Enabled);
            closeButton.onClick.AddListener(() => Enabled = false);

            //exitButton.onClick.AddListener(() => Server.Instance.StartReplaySelect());
            overlayButton.onClick.AddListener(() => overlay.SetActive(!overlay.activeSelf));
            greyboxButton.onClick.AddListener(() => replayOptions.SendAction(ReplayAction.ToggleGreyboxLabyrinth));
            algorithmButton.onClick.AddListener(() => replayOptions.SendAction(ReplayAction.ToggleAlgorithm));

            replayOptions.OnActionHandler += OnReplayAction;
            replayOptions.OnMoveIndexChangedHandler += OnMoveIndexChanged;
            replayOptions.OnCourseSelectedHandler += OnCourseSelected;

            Server.Instance.State.OnValueChangedHandler += OnGameStateChanged;

            Enabled = false;
        }

        public void OnMoveIndexChanged(int idx)
        {
            if(course != null)
            algorithmStepsText.text = algorithmStepsString.Value + course.CurrentAlgorithmMoveIndex;
        }

        public void EnableAlgorithm(bool enable)
        {
            isAlgorithmEnabled = enable;
            //algorithmNameText.gameObject.SetActive(enable);
            //algorithmStepsText.gameObject.SetActive(enable);
        }

        private Course course = null;

        public void OnReplayAction(ReplayAction action, params object[] args)
        {
            switch (action)
            {
                case ReplayAction.ToggleAlgorithm:
                    if (args.Length == 0)
                        EnableAlgorithm(!isAlgorithmEnabled);
                    else
                        EnableAlgorithm((bool)args[0]);
                    break;

                case ReplayAction.CourseToggled:
                    break;
            }
        }


        public void OnCourseSelected(Course course)
        {
            this.course = course;
            algorithmNameText.text = course.Algorithm.Name;
            algorithmStepsText.text = algorithmStepsString.Value + course.CurrentAlgorithmMoveIndex;
        }


        public void OnGameStateChanged(ServerState state)
        {
            switch (state)
            {
                case ServerState.LabyrinthReplay:
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

        private bool _enabled = false;

        public virtual bool Enabled
        {
            get
            {
                return _enabled;
            }

            set
            {
                if (Server.Instance.State.Value != ServerState.LabyrinthReplay)
                    return;

                _enabled = value;
                ButtonManager.Instance.Flags.Value =
                    _enabled ?
                    ButtonCanvasFlag.None :
                    ButtonCanvasFlag.Info | ButtonCanvasFlag.Exit;                        

                gameObject.SetActive(_enabled);
            }
        }

        private bool isOptionEnabled = false;

        public void EnableOptions(bool enable)
        {
            //isOptionEnabled = enable;
            //sequencePopup.gameObject.SetActive(isOptionEnabled);
            //sequenceToggle.SetActive(isOptionEnabled);
            //overlayButton.gameObject.SetActive(isOptionEnabled);
            ////algorithmButton.gameObject.SetActive(isOptionEnabled);
            //greyboxButton.gameObject.SetActive(isOptionEnabled);
        }

        private bool isAlgorithmEnabled = false;

    }
}