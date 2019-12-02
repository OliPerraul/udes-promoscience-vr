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
        private AvatarControllerAsset controls;

        [SerializeField]
        private Algorithms.AlgorithmRespectAsset respectController;
        
        [SerializeField]
        private GameObject waitingForNextRoundRoom;

        [SerializeField]
        private DeviceTypeManagerAsset deviceType;

        public DeviceType DeviceType => deviceType.Value;

        [SerializeField]
        public ObservableValue<Algorithms.Algorithm> Algorithm = new ObservableValue<Algorithms.Algorithm>(Algorithms.Resources.Instance.GetAlgorithm(Algorithms.Id.Standard));


        public string[] ActionValues;

        public int[] ActionSteps;

        [SerializeField]
        public ObservableValue<Labyrinths.IData> LabyrinthData = new ObservableValue<Labyrinths.IData>();

        public ObservableValue<Labyrinths.Labyrinth> Labyrinth = new ObservableValue<Labyrinths.Labyrinth>();


        public ObservableValue<ClientGameState> State = new ObservableValue<ClientGameState>();

        public void Awake()
        {
            State.Value = ClientGameState.Connecting;

            respectController.ErrorCount = 0;

            respectController.Respect = 1;

            State.OnValueChangedHandler += OnGameStateChanged;

            controls.OnPlayerReachedTheEndHandler += OnPlayerReachedTheEnd;
            
        }


        public void OnGameStateChanged(ClientGameState state)
        {
            if (
                State.Value == ClientGameState.TutorialLabyrinthReady ||
                State.Value == ClientGameState.LabyrinthReady ||
                State.Value == ClientGameState.Playing ||
                State.Value == ClientGameState.PlayingTutorial)
            {
                Labyrinth.Value.GenerateLabyrinthVisual();

                Labyrinth.Value.Init(enableCamera:false);

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
                controls.IsPlayerControlsEnabled.Value = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                State.Value = ClientGameState.WaitingReplay;
            }
            else
            {
                State.Value = ClientGameState.WaitingForNextRound;
            }
        }    
    }
}
