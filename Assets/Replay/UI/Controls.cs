using Cirrus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UdeS.Promoscience.Replay.UI
{
    public class Controls : MonoBehaviour
    {
        [SerializeField]
        private Algorithm algorithm;

        [SerializeField]
        private ScriptableObjects.ScriptableServerGameInformation server;

        [SerializeField]
        private ScriptableReplayOptions playbackOptions;

        public OnEvent OnExpandHandler;

        public OnEvent OnCloseHandler;

        [SerializeField]
        private GameObject infoPanel;

        [SerializeField]
        private GameObject navigPanel;

        [SerializeField]
        private Sprite playSprite;

        [SerializeField]
        private Sprite stopSprite;

        [SerializeField]
        private Sprite pauseSprite;

        [SerializeField]
        private Image playImage;

        [SerializeField]
        private Button playButton;

        [SerializeField]
        private Button pauseButton;

        [SerializeField]
        private Image pauseImage;

        [SerializeField]
        private Button previousButton;

        private bool sendSliderEvent = true;

        private float previousSliderValue = 0;

        [SerializeField]
        private Slider slider;

        [SerializeField]
        private Button nextButton;

        private bool enabled = false;

        public bool Enabled
        {
            get
            {
                return enabled;
            }

            set
            {
                enabled = value;
                infoPanel.SetActive(enabled);
                navigPanel.SetActive(enabled);
            }
        }

        public void OnValidate()
        {
            if (algorithm == null)
            {
                algorithm = FindObjectOfType<Algorithm>();
            }
        }

        // Use this for initialization
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            pauseButton.onClick.AddListener(OnPauseClicked);
            previousButton.onClick.AddListener(OnPreviousClicked);
            nextButton.onClick.AddListener(OnNextClicked);

            slider.onValueChanged.AddListener(OnSliderMoved);           

            playbackOptions.OnMoveIndexChanged += OnProgress;
            playbackOptions.OnSequenceFinishedHandler += OnReplaySequenceFinished;
            //playbackOptions.OnSequenceChangedHandler += OnSequenceChanged;
            playbackOptions.OnMoveCountSetHandler += OnSequenceChanged;
        }


        private void OnSequenceChanged(int mvcnt)
        {
            slider.minValue = 0;
            slider.maxValue = mvcnt;
        }


        public void OnProgress(int progress)
        {
            sendSliderEvent = false;
            slider.value = progress;
        }

        public void OnDropdown(int index)
        {
            playbackOptions.CourseIndex = index;
        }

        private bool isPlaying = false;

        private bool isPaused = false;

        public void OnPlayClicked()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                playImage.sprite = stopSprite;
                playbackOptions.SendAction(ReplayAction.Play);
            }
            else
            {
                isPlaying = false;
                playImage.sprite = playSprite;
                playbackOptions.SendAction(ReplayAction.Stop);
            }
        }

        public void OnPauseClicked()
        {
            if (!isPaused)
            {
                isPaused = true;
                playImage.sprite = playSprite;
                playbackOptions.SendAction(ReplayAction.Pause);
            }
            else
            {
                isPaused = false;
                playImage.sprite = stopSprite;
                playbackOptions.SendAction(ReplayAction.Resume);
            }
        }

        public void OnPreviousClicked()
        {
            playbackOptions.SendAction(ReplayAction.Previous);
        }

        public void OnNextClicked()
        {
            playbackOptions.SendAction(ReplayAction.Next);
        }

        public void OnSliderMoved(float value)
        {
            if (sendSliderEvent)
            {
                playbackOptions.SendAction(ReplayAction.Slide, Mathf.RoundToInt(value));
            }

            sendSliderEvent = true;
        }

        public void OnReplaySequenceFinished()
        {
            isPlaying = false;
            playImage.sprite = playSprite;
        }
    }
}
