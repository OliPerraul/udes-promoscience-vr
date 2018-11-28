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
    GameObject labyrinthRoom;

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
            
            if (labyrinthRoom != null)
            {
                labyrinthRoom.GetComponent<Animation>().Play();
            }

            if (waitingForNextRoundRoom != null)
            {
                waitingForNextRoundRoom.SetActive(false);
            }

            if (controls != null)
            {
                controls.IsControlsEnabled = true;
            }
        }
        else if (gameState.Value == ClientGameState.WaitingForNextRound)
        {
            if (waitingForNextRoundRoom != null)
            {
                waitingForNextRoundRoom.SetActive(true);
            }
        }
    }

    void OnPlayerReachedTheEnd()
    {
        controls.IsControlsEnabled = false;
        controls.StopAllMovement();
        controls.ResetPositionAndRotation();

        if (isDiverging != null)
        {
            isDiverging.Value = false;
        }

        if (labyrinthRoom != null)
        {
            labyrinthRoom.transform.position = new Vector3(0, 0, 0);
        }

        if (gameState.Value == ClientGameState.PlayingTutorial)
        {
            gameState.Value = ClientGameState.TutorialLabyrinthReady;
        }
        else
        {
            gameState.Value = ClientGameState.WaitingForNextRound;
        }
    }

}
