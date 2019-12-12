//using UdeS.Promoscience.Utils;
using System;
using System.Collections.Generic;
using UdeS.Promoscience.Labyrinths;
using UdeS.Promoscience.Network;
using UnityEngine.SceneManagement;

using UnityEngine;
using Cirrus;
using System.Linq;

namespace UdeS.Promoscience
{
    [System.Serializable]
    public class ObservableServerState : ObservableValue<ServerState> { public ObservableServerState(ServerState state) : base(state) { } }

    public class Server : BaseSingleton<Server>
    {
        /// TODODODODODOO

        // TODO Labyrinth manager??/


        /// TODODODODODOO




        [SerializeField]
        private ServerSettings settings;

        public ServerSettings Settings => settings;

        private Replays.ControlReplay replay;

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
                settings = new ServerSettings();
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
                case ServerState.LevelReplay:
                    break;

                default:
                    //playbackOptions.Courses.Clear();
                    break;
            }

        }

        public void OnApplicationQuit()
        {
            // TODO remove from here
            SQLiteUtilities.ResetServerPlayerInformation();
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
            Replays.ReplayManager.Instance.StartCurrentLevelReplay();
        }

        public void StartAdvancedReplay()
        {
            Replays.ReplayManager.Instance.StartGameReplay();
        }


        // Try find course ID initiated by a team member
        // Otherwise assign new course
        // Returns true if created a course
        public void AssignCourse(Player player)//, out Course course)
        {
            GameManager.Instance.CurrentGame.AssignCourse(player);
        }

        public void StartNextLevel()
        {
            GameManager.Instance.CurrentGame.StartNextLevel();
        }



        public void ReturnToGame()
        {
            if (GameManager.Instance.CurrentGame == null || 
                GameManager.Instance.CurrentGame.CurrentLevel == null)
            {
                ReturnToLobby();
            }
            else
            {
                Instance.State.Value = GameManager.Instance.CurrentGame.LevelState;
            }
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

