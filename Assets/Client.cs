using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Controls;
using System;
using Cirrus;
using Cirrus.Extensions;
//using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    public class Client : BaseSingleton<Client>
    {
        [SerializeField]
        private ClientSettings settings;

        public ClientSettings Settings => settings;

        [SerializeField]
        private ControlsAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset respectController;
        
        [SerializeField]
        private GameObject waitingForNextRoundRoom;

        [SerializeField]
        private DeviceTypeManagerAsset deviceType;

        public DeviceType DeviceType => deviceType.Value;

        [SerializeField]
        public ObservableValue<Algorithms.Algorithm> Algorithm = new ObservableValue<Algorithms.Algorithm>();

        
        private DateTime roundBeginTime;

        public DateTime RoundBeginTime => roundBeginTime;


        public string[] ActionValues;

        public int[] ActionSteps;

        [SerializeField]
        public ObservableValue<Labyrinths.ILabyrinth> LabyrinthData = new ObservableValue<Labyrinths.ILabyrinth>();

        public ObservableValue<Labyrinths.LabyrinthObject> Labyrinth = new ObservableValue<Labyrinths.LabyrinthObject>();


        public ObservableValue<ClientGameState> State = new ObservableValue<ClientGameState>(ClientGameState.NotReady);

        public void Awake()
        {
            if (settings == null)
            {
                settings = new ClientSettings();
            }

            settings.LoadFromPlayerPrefs();


            State.OnValueChangedHandler += OnGameStateChanged;

            respectController.ErrorCount = 0;

            respectController.Respect = 1;

            controls.OnPlayerReachedTheEndHandler += OnPlayerReachedTheEnd;
            
        }

        public void OnGameStateChanged(ClientGameState state)
        {
            if (State.Value == ClientGameState.Playing)
            {
                Labyrinth.Value.GenerateLabyrinthVisual();
                Labyrinth.Value.Init(enableCamera:false);
                roundBeginTime = DateTime.Now;

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
            else if (State.Value == ClientGameState.ViewingLocalReplay)
            {
                //gameCamera.ChangeState(Camera.State.Topdown);

                controls.IsPlayerControlsEnabled.Value = false;
                controls.StopAllMovement();
            }
            else if (State.Value == ClientGameState.WaitingForNextRound)
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

            if (State.Value == ClientGameState.PlayingTutorial ||
                State.Value == ClientGameState.Playing)
            {
                Destroy(Labyrinth.Value.gameObject);

                controls.IsPlayerControlsEnabled.Value = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                State.Value = ClientGameState.WaitingForNextRound;
            }
        }    
    }
}
