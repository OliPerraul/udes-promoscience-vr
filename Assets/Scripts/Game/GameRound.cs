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
    GameObject lobby;

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

            if (lobby != null)
            {
                lobby.SetActive(false);
            }

            if (controls != null)
            {
                controls.IsControlsEnabled = true;
            }
        }
        else if (gameState.Value == ClientGameState.WaitingForNextRound)
        {
            controls.IsControlsEnabled = false;
            controls.StopAllMovement();
            controls.ResetPositionAndRotation();

            labyrinthRoom.transform.position = new Vector3(0, 0, 0);

            if (lobby != null)
            {
                lobby.SetActive(true);
            }

            if(isDiverging != null)
            {
                isDiverging.Value = false;
            }
        }
    }

    void OnPlayerReachedTheEnd()
    {
        if (gameState.Value == ClientGameState.PlayingTutorial)
        {
            controls.IsControlsEnabled = false;
            controls.StopAllMovement();
            controls.ResetPositionAndRotation();

            labyrinthRoom.transform.position = new Vector3(0, 0, 0);

            if (isDiverging != null)
            {
                isDiverging.Value = false;
            }

            gameState.Value = ClientGameState.TutorialLabyrinthReady;
        }
        else
        {
            gameState.Value = ClientGameState.WaitingForNextRound;
        }
    }

}
