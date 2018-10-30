using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRound : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;

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
        if (gameState.Value == GameState.Playing || gameState.Value == GameState.PlayingTutorial)
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
    }

    void OnPlayerReachedTheEnd()
    {
        controls.IsControlsEnabled = false;
        controls.StopAllMovement();
        controls.ResetPositionAndRotation();

        labyrinthRoom.transform.position = new Vector3(0, 0, 0);

        if (gameState.Value == GameState.PlayingTutorial)
        {
            gameState.Value = GameState.TutorialLabyrinthReady;
        }
        else
        {
            if (lobby != null)
            {
                lobby.SetActive(true);
            }

            gameState.Value = GameState.WaitingForNextRound;
        }
    }

}
