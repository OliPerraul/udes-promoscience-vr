using Cirrus;
using Cirrus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UdeS.Promoscience.Replays.UI
{
    public class Controls : MonoBehaviour
    {
        [SerializeField]
        private ReplayManagerAsset playbackOptions;

        public Cirrus.Event OnExpandHandler;

        public Cirrus.Event OnCloseHandler;

        //[SerializeField]
        //private GameObject infoPanel;

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
        private Button fastfowardButton;

        [SerializeField]
        private Text playbackSpeedText;

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
                //infoPanel.SetActive(enabled);
                navigPanel.SetActive(enabled);
            }
        }

        public void OnValidate()
        {

        }

        // Use this for initialization
        private void Awake()
        {
            playButton.onClick.AddListener(OnPlayClicked);
            pauseButton.onClick.AddListener(OnPauseClicked);
            previousButton.onClick.AddListener(OnPreviousClicked);
            nextButton.onClick.AddListener(OnNextClicked);
            fastfowardButton.onClick.AddListener(OnFastFowardClicked);
            slider.onValueChanged.AddListener(OnSliderMoved);           

            playbackOptions.OnMoveIndexChangedHandler += OnProgress;
            playbackOptions.OnSequenceFinishedHandler += OnReplaySequenceFinished;            
            playbackOptions.OnMoveCountSetHandler += OnSequenceChanged;
            playbackOptions.OnPlaybackSpeedHandler += OnPlaybackSpeedChanged;
        }


        public void OnDestroy()
        {
            playbackOptions.OnMoveIndexChangedHandler -= OnProgress;
            playbackOptions.OnSequenceFinishedHandler -= OnReplaySequenceFinished;
            playbackOptions.OnMoveCountSetHandler -= OnSequenceChanged;
            playbackOptions.OnPlaybackSpeedHandler -= OnPlaybackSpeedChanged;
        }

        public void OnPlaybackSpeedChanged(float value)
        {
            playbackSpeedText.text = "x" + playbackOptions.PlaybackSpeed;
        }

        private void OnFastFowardClicked()
        {
            playbackOptions.PlaybackSpeed += 0.5f;
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
                playbackOptions.SendAction(ReplayControlAction.Play);
            }
            else
            {
                isPlaying = false;
                playImage.sprite = playSprite;
                playbackOptions.SendAction(ReplayControlAction.Stop);
            }
        }

        public void OnPauseClicked()
        {
            if (!isPaused)
            {
                isPaused = true;
                playImage.sprite = playSprite;
                playbackOptions.SendAction(ReplayControlAction.Pause);
            }
            else
            {
                isPaused = false;
                playImage.sprite = stopSprite;
                playbackOptions.SendAction(ReplayControlAction.Resume);
            }
        }

        public void OnPreviousClicked()
        {
            playbackOptions.SendAction(ReplayControlAction.Previous);
        }

        public void OnNextClicked()
        {
            playbackOptions.SendAction(ReplayControlAction.Next);
        }

        public void OnSliderMoved(float value)
        {
            if (sendSliderEvent)
            {
                playbackOptions.SendAction(ReplayControlAction.Slide, Mathf.RoundToInt(value));
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
