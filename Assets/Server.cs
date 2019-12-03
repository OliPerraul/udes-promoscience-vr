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
    [System.Serializable]
    public class ObservableServerState : ObservableValue<ServerState> { public ObservableServerState(ServerState state) : base(state) { } }

    public class Server : BaseSingleton<Server>
    {
        [SerializeField]
        private Settings settings;

        public Settings Settings => settings;

        private Replays.BaseReplay replay;

        public Game CurrentGame { get; private set; }

        [SerializeField]
        public ObservableServerState State = new ObservableServerState(ServerState.Lobby);

        public static bool IsApplicationServer => Instance == null;

        [SerializeField]
        private SceneWrapperAsset lobbyScene;

        [SerializeField]
        private SceneWrapperAsset menuScene;

        private UnityEngine.Events.UnityAction<Scene, LoadSceneMode> onSceneLoadedCallback;

        public void Awake()
        {
            Persist();

            if (settings == null)
            {
                settings = new Settings();
            }

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
            State.Set(
                SceneManager.GetActiveScene().path == lobbyScene.ScenePath ?
                ServerState.Lobby :
                ServerState.Menu,
                notify: false);
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

        public void StartAdvancedReplay()
        {
            Replays.ReplayManager.Instance.StartReplaySelect();
        }


        // Try find course ID initiated by a team member
        // Otherwise assign new course
        // Returns true if created a course
        public void AssignCourse(Player player)//, out Course course)
        {
            GameManager.Instance.CurrentGame.AssignCourse(player);
        }

        public void StartNextRound()
        {
            GameManager.Instance.CurrentGame.StartNextRound();
        }

        public void ReturnToGame()
        {
            Instance.State.Value = GameManager.Instance.CurrentGame.RoundState;
        }

        public void ReturnToLobby()
        {
            Instance.State.Value = ServerState.Lobby;
        }

        public void StopGame()
        {
            GameManager.Instance.StopGame();
        }


        public void StartLobby()
        {
            SceneManager.sceneLoaded -= onSceneLoadedCallback;
            onSceneLoadedCallback = new UnityEngine.Events.UnityAction<Scene, LoadSceneMode>(
                (Scene scene, LoadSceneMode mode) => Instance.State.Value = ServerState.Lobby);

            SceneManager.sceneLoaded += onSceneLoadedCallback;

            lobbyScene.Load();
        }

        public void StartMenu()
        {
            SceneManager.sceneLoaded -= onSceneLoadedCallback;
            onSceneLoadedCallback = new UnityEngine.Events.UnityAction<Scene, LoadSceneMode>(
                (Scene scene, LoadSceneMode mode) => Instance.State.Value = ServerState.Menu);
            SceneManager.sceneLoaded += onSceneLoadedCallback;

            menuScene.Load();
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

