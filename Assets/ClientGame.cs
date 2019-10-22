using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    public class ClientGame : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableClientGameState client;               

        [SerializeField]
        ScriptableAction playerReachedTheEnd;

        [SerializeField]
        ScriptableBoolean isDiverging;
        
        [SerializeField]
        GameObject waitingForNextRoundRoom;

        [SerializeField]
        private ScriptableIntegerArray recordedSteps;


        void Start()
        {
            client.clientStateChangedEvent += OnGameStateChanged;

            if (playerReachedTheEnd != null)
            {
                playerReachedTheEnd.action += OnPlayerReachedTheEnd;
            }
        }

        public void OnGameStateChanged()
        {

            if (
                client.Value == ClientGameState.TutorialLabyrinthReady ||
                client.Value == ClientGameState.LabyrinthReady)
            {
                client.Labyrinth.GenerateLabyrinthVisual();
                client.Value = ClientGameState.PlayingTutorial;
            }
            else if (
                client.Value == ClientGameState.Playing ||
                client.Value == ClientGameState.PlayingTutorial)
            {
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
            else if (client.Value == ClientGameState.ViewingLocalReplay)
            {
                //gameCamera.ChangeState(Camera.State.Topdown);

                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
            }
            else if (client.Value == ClientGameState.WaitingForNextRound)
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

            if (client.Value == ClientGameState.PlayingTutorial ||
                client.Value == ClientGameState.Playing)
            {
                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                client.Value = ClientGameState.WaitingReplay;
            }
            else
            {
                client.Value = ClientGameState.WaitingForNextRound;
            }
        }
    }
}
