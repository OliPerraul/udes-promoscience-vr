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
    public class Client : MonoBehaviour
    {
        public static Client Instance;

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

        public OnEvent OnAlgorithmChangedHandler;

        [SerializeField]
        private Algorithms.Algorithm algorithm;

        public Algorithms.Algorithm Algorithm
        {
            get
            {
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
            Instance = this;

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
