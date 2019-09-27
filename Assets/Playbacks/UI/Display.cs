//using Cirrus.Extensions;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using UnityEngine.EventSystems;
//using UnityEngine.UI;

//namespace UdeS.Promoscience.Playbacks.UI
//{

//    public class Display : MonoBehaviour
//    {
//        public delegate void OnButton();

//        [SerializeField]
//        private Algorithm algorithm;

//        [SerializeField]
//        private ScriptableObjects.ScriptableServerGameInformation server;

//        [SerializeField]
//        private ScriptablePlaybackOptions playbackOptions;

//        public OnButton OnExpandHandler;

//        public OnButton OnCloseHandler;

//        [SerializeField]
//        private GameObject infoPanel;

//        [SerializeField]
//        private GameObject navigPanel;

//        [SerializeField]
//        private Dropdown dropdown;

//        [SerializeField]
//        private Sprite playSprite;

//        [SerializeField]
//        private Sprite stopSprite;

//        [SerializeField]
//        private Sprite pauseSprite;

//        [SerializeField]
//        private Image playImage;

//        [SerializeField]
//        private Button playButton;

//        [SerializeField]
//        private Button pauseButton;

//        [SerializeField]
//        private Image pauseImage;

//        [SerializeField]
//        private Button previousButton;

//        private bool sendSliderEvent = true;

//        private float previousSliderValue = 0;

//        [SerializeField]
//        private Slider slider;

//        [SerializeField]
//        private Button nextButton;

//        private bool enabled = false;

//        public bool Enabled
//        {
//            get
//            {
//                return enabled;
//            }

//            set
//            {
//                enabled = value;
//                infoPanel.SetActive(enabled);
//                navigPanel.SetActive(enabled);
//            }
//        }

//        public void OnValidate()
//        {
//            if (algorithm == null)
//            {
//                algorithm = FindObjectOfType<Algorithm>();
//            }
//        }

//        // Use this for initialization
//        private void Awake()
//        {
//            server.gameStateChangedEvent += OnGameStateChanged;
//            dropdown.ClearOptions();

//            dropdown.onValueChanged.AddListener(OnDropdown);

//            playButton.onClick.AddListener(OnPlayClicked);
//            pauseButton.onClick.AddListener(OnPauseClicked);
//            previousButton.onClick.AddListener(OnPreviousClicked);
//            nextButton.onClick.AddListener(OnNextClicked);

//            slider.onValueChanged.AddListener(OnSliderMoved);


//            playbackOptions.OnProgressHandler += OnProgress;
//        }

//        public void OnGameStateChanged()
//        {
//            switch (server.GameState)
//            {
//                case Utils.ServerGameState.ViewingPlayback:

//                    dropdown.ClearOptions();

//                    List<Dropdown.OptionData> options = new List<Dropdown.OptionData>();
//                    foreach (CourseData course in playbackOptions.Courses)
//                    {
//                        options.Add(new Dropdown.OptionData(
//                            course.Team.name + " (" +
//                            ScriptableObjects.ScriptableAlgorithm.Instance.GetName(course.Algorithm) +
//                            ") "                            
//                            ));
//                    }                    
                    
//                    dropdown.AddOptions(options);
//                    playbackOptions.CourseIndex = 0;

//                break;
//            }
//        }

//        public void OnProgress(float progress)
//        {
//            sendSliderEvent = false;
//            slider.value = progress;
//        }

//        public void OnDropdown(int index)
//        {
//            playbackOptions.CourseIndex = index;
//        }

//        private bool isPlaying = false;

//        private bool isPaused = false;

//        public void OnPlayClicked()
//        {
//            if (!isPlaying)
//            {
//                isPlaying = true;
//                playImage.sprite = stopSprite;
//                playbackOptions.SendAction(PlaybackAction.Play);
//            }
//            else
//            {
//                isPlaying = false;
//                playImage.sprite = playSprite;
//                playbackOptions.SendAction(PlaybackAction.Stop);
//            }
//        }

//        public void OnPauseClicked()
//        {
//            if (!isPaused)
//            {
//                isPaused = true;
//                playImage.sprite = playSprite;
//                playbackOptions.SendAction(PlaybackAction.Pause);
//            }
//            else
//            {
//                isPaused = false;
//                playImage.sprite = stopSprite;
//                playbackOptions.SendAction(PlaybackAction.Resume);
//            }
//        }

//        public void OnPreviousClicked()
//        {
//            playbackOptions.SendAction(PlaybackAction.Previous);
//        }

//        public void OnNextClicked()
//        {
//            playbackOptions.SendAction(PlaybackAction.Next);
//        }

//        public void OnSliderMoved(float value)
//        {
//            if (sendSliderEvent)
//            {
//                playbackOptions.SendAction(PlaybackAction.Slide, previousSliderValue, value);
//            }

//            sendSliderEvent = true;
//            previousSliderValue = value;
//        }

//    }
//}
