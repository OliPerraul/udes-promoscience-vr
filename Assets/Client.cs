using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using System;
using Cirrus;
using Cirrus.Extensions;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    public class Client : BaseSingleton<Client>
    {
        [SerializeField]
        private ScriptableControler controls;

        //// TODO combine with this class
        //[SerializeField]
        //public ScriptableClientGameState client;

        [SerializeField]
        private ScriptableAction playerReachedTheEnd;

        [SerializeField]
        private ScriptableBoolean isDiverging;
        
        [SerializeField]
        private GameObject waitingForNextRoundRoom;

        [SerializeField]
        private ScriptableIntegerArray recordedSteps;

        // This is ridiculous..
        // TODO: remove all scriptableVariables put them in client or somewhere else..
        // Global vars are bad.. At least put them in one place!
        [SerializeField]
        private ScriptableDeviceType deviceType;

        public OnEvent OnAlgorithmChangedHandler;

        public DeviceType DeviceType
        {
            get
            {
                return deviceType.Value;
            }
        }

        [SerializeField]
        private Algorithms.Algorithm algorithm;

        public Algorithms.Algorithm Algorithm
        {
            get
            {
                if (algorithm == null)
                    algorithm = Algorithms.Resources.Instance.GetAlgorithm(Algorithms.Id.Standard);

                return algorithm;
            }

            set
            {
                algorithm = value;

                if (OnAlgorithmChangedHandler != null)
                    OnAlgorithmChangedHandler.Invoke();
            }
        }

        [SerializeField]
        private ClientGameState value;

        public Action clientStateChangedEvent;

        public string[] ActionValues;

        public int[] ActionSteps;
        
        public OnFloatEvent OnRespectChangedHandler;

        public OnEvent OnLabyrinthChangedHandler;

        [SerializeField]
        private Labyrinths.IData labyrinthData;

        public Labyrinths.IData LabyrinthData
        {
            get
            {
                return labyrinthData;
            }

            set
            {
                labyrinthData = value;

                if (OnLabyrinthChangedHandler != null)
                    OnLabyrinthChangedHandler.Invoke();
            }
        }

        private Labyrinths.Labyrinth labyrinth;

        public Labyrinths.Labyrinth Labyrinth
        {
            get
            {
                return labyrinth;
            }

            set
            {
                if (labyrinth != null)
                    labyrinth.gameObject.Destroy();

                labyrinth = value;
            }
        }

        public int ErrorCount = 0;

        private float respect;

        public float Respect
        {
            get
            {
                return respect;
            }

            set
            {
                if (respect.Approximately(value))
                    return;

                respect = value;
                if (OnRespectChangedHandler != null)
                {
                    OnRespectChangedHandler.Invoke(respect);
                }
            }
        }

        public ClientGameState State
        {
            get
            {
                return value;
            }
            set
            {
                this.value = value;
                OnValueChanged();
            }
        }

        public void Awake()
        {
            value = ClientGameState.Connecting;

            ErrorCount = 0;

            respect = 1;

            clientStateChangedEvent += OnGameStateChanged;

            if (playerReachedTheEnd != null)
            {
                playerReachedTheEnd.action += OnPlayerReachedTheEnd;
            }
        }


        public void OnGameStateChanged()
        {
            if (
                State == ClientGameState.TutorialLabyrinthReady ||
                State == ClientGameState.LabyrinthReady ||
                State == ClientGameState.Playing ||
                State == ClientGameState.PlayingTutorial)
            {
                Labyrinth.GenerateLabyrinthVisual();

                Labyrinth.Init();

                if (controls != null)
                {
                    controls.StopAllMovement();
                    controls.ResetPositionAndRotation();
                }

                if (waitingForNextRoundRoom != null)
                {
                    waitingForNextRoundRoom.SetActive(false);
                }

                if (controls != null)
                {
                    controls.IsPlayerControlsEnabled = true;
                }
            }
            else if (State == ClientGameState.ViewingLocalReplay)
            {
                //gameCamera.ChangeState(Camera.State.Topdown);

                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
            }
            else if (State == ClientGameState.WaitingForNextRound)
            {
                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();

                if (waitingForNextRoundRoom != null)
                {
                    waitingForNextRoundRoom.SetActive(true);
                }
            }
        }

        void OnPlayerReachedTheEnd()
        {
            if (isDiverging != null)
            {
                isDiverging.Value = false;
            }

            if (State == ClientGameState.PlayingTutorial ||
                State == ClientGameState.Playing)
            {
                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                State = ClientGameState.WaitingReplay;
            }
            else
            {
                State = ClientGameState.WaitingForNextRound;
            }
        }

        public void OnValueChanged()
        {
            if (clientStateChangedEvent != null)
            {
                clientStateChangedEvent();
            }
        }
    
    }
}
