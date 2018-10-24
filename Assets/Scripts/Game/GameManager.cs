using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manage the interaction from to game to the network player
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    ScriptableGameState gameState;

    [SerializeField]
    ScriptableAction playerReachedTheEnd;

    [SerializeField]
    AlgorithmRespect algorithmRespect;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    GameObject labyrinthRoom;

    [SerializeField]
    GameObject lobby;//should probably manage it's self or just not exist

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//Tablet use only

        gameState.valueChangedEvent += OnGameStateChanged;

        if (playerReachedTheEnd != null)
        {
            playerReachedTheEnd.action += OnPlayerReachedTheEnd;
        }
    }

    //Game manager could be remove and instead add a status preparing for game and have the module that need it listen to status
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
        else if (gameState.Value == GameState.WaitingForNextRound)
        {

        }
    }

    void OnPlayerReachedTheEnd()
    {
        if (gameState.Value == GameState.PlayingTutorial)
        {
            controls.IsControlsEnabled = false;
            controls.StopAllMovement();
            controls.ResetPositionAndRotation();

            labyrinthRoom.transform.position = new Vector3(0, 0, 0);

            gameState.Value = GameState.TutorialLabyrinthReady;
        }
        else
        {
            controls.IsControlsEnabled = false;
            controls.StopAllMovement();
            controls.ResetPositionAndRotation();

            labyrinth.DestroyLabyrinth();
            labyrinthRoom.transform.position = new Vector3(0, 0, 0);

            if (lobby != null)
            {
                lobby.SetActive(true);
            }

            gameState.Value = GameState.WaitingForNextRound;
        }
    }

}
