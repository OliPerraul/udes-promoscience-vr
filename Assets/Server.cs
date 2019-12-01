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
    public class Server : BaseSingleton<Server>
    {
        private Replays.BaseReplay replay;

        public Game CurrentGame { get; private set; }

        public ObservableValue<ServerState> State = new ObservableValue<ServerState>();

        [SerializeField]
        private ServerState state;

        public static bool IsApplicationServer => Instance == null;

        public void Awake()
        {
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
            this.state = state;

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

