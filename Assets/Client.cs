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
        private AvatarControllerAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset respectController;
        
        [SerializeField]
        private GameObject waitingForNextRoundRoom;

        [SerializeField]
        private DeviceTypeManagerAsset deviceType;

        public Cirrus.Event OnAlgorithmChangedHandler;

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
        

        public Cirrus.Event OnLabyrinthChangedHandler;

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

            respectController.ErrorCount = 0;

            respectController.Respect = 1;

            clientStateChangedEvent += OnGameStateChanged;

            controls.OnPlayerReachedTheEndHandler += OnPlayerReachedTheEnd;
            
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
                    controls.IsPlayerControlsEnabled.Value = true;
                }
            }
            else if (State == ClientGameState.ViewingLocalReplay)
            {
                //gameCamera.ChangeState(Camera.State.Topdown);

                controls.IsPlayerControlsEnabled.Value = false;
                controls.StopAllMovement();
            }
            else if (State == ClientGameState.WaitingForNextRound)
            {
                controls.IsPlayerControlsEnabled.Value = false;
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
            if (respectController.IsDiverging != null)
            {
                respectController.IsDiverging.Value = false;
            }

            if (State == ClientGameState.PlayingTutorial ||
                State == ClientGameState.Playing)
            {
                controls.IsPlayerControlsEnabled.Value = false;
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
