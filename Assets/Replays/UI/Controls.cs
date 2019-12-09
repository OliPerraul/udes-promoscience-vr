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
        private ReplayControlsAsset asset;

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

        //[SerializeField]
        //private ControlsServerResponseAsset serverResponse;

        private bool enabled = false;

        public bool Enabled
        {
            get => enabled;
                      
            set
            {
                enabled = value;
                navigPanel.SetActive(enabled);
            }
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

            asset.ReplayMoveCount.OnValueChangedHandler += (x) => slider.maxValue = x;
            asset.ReplayMoveIndexChangedHandler += (x) => slider.SetValueWithoutNotify(x);
        }

        public void OnPlaybackSpeedChanged(float value)
        {
            //playbackSpeedText.text = "x" + replay.PlaybackSpeed;
        }

        private void OnFastFowardClicked()
        {
            //replay.PlaybackSpeed += 0.5f;
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
            //replay.CourseIndex = index;
        }

        private bool isPlaying = false;

        private bool isPaused = false;

        public void OnPlayClicked()
        {
            if (!isPlaying)
            {
                isPlaying = true;
                playImage.sprite = stopSprite;
                asset.SendAction(ReplayControlAction.Play);
            }
            else
            {
                isPlaying = false;
                playImage.sprite = playSprite;
                asset.SendAction(ReplayControlAction.Stop);
            }
        }

        public void OnPauseClicked()
        {
            if (!isPaused)
            {
                isPaused = true;
                playImage.sprite = playSprite;
                asset.SendAction(ReplayControlAction.Pause);
            }
            else
            {
                isPaused = false;
                playImage.sprite = stopSprite;
                asset.SendAction(ReplayControlAction.Resume);
            }
        }

        public void OnPreviousClicked()
        {
            asset.SendAction(ReplayControlAction.Previous);
        }

        public void OnNextClicked()
        {
            asset.SendAction(ReplayControlAction.Next);
        }

        public void OnSliderMoved(float value)
        {
            if (sendSliderEvent)
            {
                asset.SlideValue.Value = Mathf.RoundToInt(value);
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
