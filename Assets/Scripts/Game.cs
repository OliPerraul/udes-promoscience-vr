using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;

namespace UdeS.Promoscience
{
    public class Game : MonoBehaviour
    {
        [SerializeField]
        ScriptableControler controls;

        [SerializeField]
        ScriptableClientGameState gameState;

        [SerializeField]
        ScriptableAction playerReachedTheEnd;

        [SerializeField]
        ScriptableBoolean isDiverging;

        [SerializeField]
        Labyrinth labyrinth;

        [SerializeField]
        GameObject waitingForNextRoundRoom;

        [SerializeField]
        private Camera gameCamera;

        [SerializeField]
        private ScriptableIntegerArray recordedSteps;

        [SerializeField]
        private PlaybackManager playbackManager;

        void Start()
        {
            gameState.valueChangedEvent += OnGameStateChanged;

            if (playerReachedTheEnd != null)
            {
                playerReachedTheEnd.action += OnPlayerReachedTheEnd;
            }
        }

        public void OnGameStateChanged()
        {
            if (gameState.Value == ClientGameState.Playing || gameState.Value == ClientGameState.PlayingTutorial)
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
            else if (gameState.Value == ClientGameState.ViewingPlayback)
            {
                gameCamera.ChangeState(Camera.State.Topdown);
                playbackManager.BeginPlayback();

                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
            }
            else if (gameState.Value == ClientGameState.WaitingForNextRound)
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

            if (gameState.Value == ClientGameState.PlayingTutorial)
            {
                controls.IsPlayerControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                gameState.Value = ClientGameState.WaitingPlayback;
            }
            else
            {
                gameState.Value = ClientGameState.WaitingForNextRound;
            }
        }
    }
}
