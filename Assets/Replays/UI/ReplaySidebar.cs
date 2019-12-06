using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UdeS.Promoscience.UI;

namespace UdeS.Promoscience.Replays.UI
{
    public class ReplaySidebar : MonoBehaviour
    {
        //[SerializeField]
        //protected ReplayManagerAsset replayOptions;

        [SerializeField]
        private UnityEngine.UI.Text algorithmNameText;

        [SerializeField]
        private UnityEngine.UI.Text algorithmStepsText;

        [SerializeField]
        private UnityEngine.UI.Button algorithmNext;

        [SerializeField]
        private UnityEngine.UI.Button algorithmPrevious;


        //[SerializeField]
        //private LocalizeInlineString algorithmStepsString = new LocalizeInlineString("Number of steps: ");

        [SerializeField]
        protected Controls controls;

        //[SerializeField]
        //[UnityEngine.Serialization.FormerlySerializedAs("openButton")]
        //protected UnityEngine.UI.Button infoButton;

        [SerializeField]
        protected UnityEngine.UI.Button closeButton;

        //[SerializeField]
        //protected UnityEngine.UI.Button exitButton;

        [SerializeField]
        protected UnityEngine.UI.Button overlayButton;

        [SerializeField]
        protected UnityEngine.UI.Button greyboxButton;

        [SerializeField]
        protected UnityEngine.UI.Button algorithmButton;


        [SerializeField]
        protected GameObject sequenceToggle;

        //[SerializeField]
        //protected SelectedSequenceDisplay sequencePopup;

        [SerializeField]
        private GameObject overlay;

        private LabyrinthReplay replay;


        public virtual void Awake()
        {
            Server.Instance.State.OnValueChangedHandler += OnGameStateChanged;
            ButtonManager.Instance.InfoButton.onClick.AddListener(() => Enabled = !Enabled);
            ReplayManager.Instance.OnLabyrinthReplayStartedHandler += OnReplayStarted;

            closeButton.onClick.AddListener(() => Enabled = false);

            Enabled = false;
        }

        public virtual void OnDestroy()
        {
            Server.Instance.State.OnValueChangedHandler -= OnGameStateChanged;

            if (replay != null)
            {
                
                replay.OnMoveIndexChangedHandler -= OnMoveIndexChanged;
                replay.CurrentCourse.OnValueChangedHandler -= OnCourseSelected;
            }
        }


        public void OnReplayStarted(BaseReplay replay)
        {
            if (this.replay != null)
            {
                this.replay.OnMoveIndexChangedHandler -= OnMoveIndexChanged;
                this.replay.CurrentCourse.OnValueChangedHandler -= OnCourseSelected;
            }

            this.replay.OnMoveIndexChangedHandler += OnMoveIndexChanged;
            this.replay.CurrentCourse.OnValueChangedHandler += OnCourseSelected;
        }


        public void OnMoveIndexChanged(int idx)
        {
            if(course != null)
            algorithmStepsText.text = course.CurrentAlgorithmMoveIndex.ToString();
        }

        public void EnableAlgorithm(bool enable)
        {
            isAlgorithmEnabled = enable;
            //algorithmNameText.gameObject.SetActive(enable);
            //algorithmStepsText.gameObject.SetActive(enable);
        }

        private Course course = null;


        public void OnCourseSelected(Course course)
        {
            this.course = course;
            algorithmNameText.text = course.Algorithm.Name;
            algorithmStepsText.text = course.CurrentAlgorithmMoveIndex.ToString();
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