using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRound : MonoBehaviour
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
    GameLabyrinth labyrinth;

    [SerializeField]
    GameObject waitingForNextRoundRoom;

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

            gameState.Value = ClientGameState.TutorialLabyrinthReady;
        }
        else
        {
            gameState.Value = ClientGameState.WaitingForNextRound;
        }
    }

}
