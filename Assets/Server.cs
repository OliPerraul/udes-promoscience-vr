//using UdeS.Promoscience.Utils;
using System;
using System.Collections.Generic;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using UnityEngine.SceneManagement;

using UnityEngine;
using Cirrus;

namespace UdeS.Promoscience
{
    // TODO client settings?
    [Serializable]
    public class Settings
    {
        public const string PredefinedLevelOrderString = "PredefinedLevelOrder";

        [SerializeField]
        public ObservableBool PredefinedLevelOrder = new ObservableBool(false);


        public const int MinNumberOfRounds = 3;

        public const int MaxNumberOfRounds = 10;

        public const string NumberOfRoundsString = "NumberOfRounds";

        [SerializeField]
        public ObservableInt NumberOfRounds = new ObservableInt(MinNumberOfRounds);

        public Settings()
        {
            PredefinedLevelOrder.OnValueChangedHandler += 
                (x) => OnSettingChanged(PredefinedLevelOrderString, x);

            NumberOfRounds.OnValueChangedHandler += 
                (x) => OnSettingChanged(NumberOfRoundsString, Mathf.Clamp(x, MinNumberOfRounds, MaxNumberOfRounds));
        }

        public void OnSettingChanged(string setting, bool enabled)
        {
            PlayerPrefs.SetInt(setting, enabled ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void OnSettingChanged(string setting, int value)
        {
            PlayerPrefs.SetInt(setting, value);
            PlayerPrefs.Save();
        }

        public void LoadFromPlayerPrefs()
        {
            if (PlayerPrefs.HasKey(PredefinedLevelOrderString))
                PredefinedLevelOrder.Set(PlayerPrefs.GetInt(PredefinedLevelOrderString) == 1, notify: false);

            if (PlayerPrefs.HasKey(NumberOfRoundsString))
                NumberOfRounds.Set(PlayerPrefs.GetInt(NumberOfRoundsString), notify: false);
        }
    }

    public class Server : BaseSingleton<Server>
    {
        [SerializeField]
        private Settings settings;

        public Settings Settings => settings;

        private Replays.BaseReplay replay;

        public Game CurrentGame { get; private set; }

        public ObservableValue<ServerState> State = new ObservableValue<ServerState>();

        public static bool IsApplicationServer => Instance == null;

        public void Awake()
        {
            Persist();

            settings = new Settings();
            settings.LoadFromPlayerPrefs();

            // Randomize seed
            UnityEngine.Random.InitState((int)Time.time);

            // Store all the teams in the DB
            // TODO remove, replace with 'Resources' asset
            foreach (Teams.TeamResource team in Teams.Resources.Instance.Teams)
            {
                SQLiteUtilities.InsertTeam(
                    team.TeamId,
                    team.TeamName,
                    team.TeamColor.ToString(),
                    DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff"));
            }

            State.OnValueChangedHandler += OnStateValueChanged;                    
        }

        public void Start()
        {
            State.Set(ServerState.Lobby);
        }

        public void OnStateValueChanged(ServerState state)
        {
            //this.state = state;

            switch (state)
            {
                case ServerState.LabyrinthReplay:
                    break;

                default:
                    //playbackOptions.Courses.Clear();
                    break;
            }

        }

        public void StartQuickplay()
        {
            GameManager.Instance.StartQuickplay();
        }

        public void StartGame()
        {
            GameManager.Instance.StartNewGame();
        }

        public void StartInstantReplay()
        {
            Replays.ReplayManager.Instance.StartInstantReplay();
        }



        // Try find course ID initiated by a team member
        // Otherwise assign new course
        // Returns true if created a course
        public void AssignCourse(Player player)//, out Course course)
        {
            GameManager.Instance.CurrentGame.AssignCourse(player);
        }


        public void LoadGameInformationFromDatabase()
        {
            //SQLiteUtilities.SetServerGameInformation(this);
            //gameStateChangedEvent += SaveGameInformationToDatabase;//Work only because gameRound is always updated right before gameState
        }



        public void SaveGameInformationToDatabase()
        {
            //SQLiteUtilities.InsertServerGameInformation(this);            
        }



        public void ClearGameInformation()
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            //SQLiteUtilities.ResetServerGameInformation();
#endif
        }

    
    }
}

