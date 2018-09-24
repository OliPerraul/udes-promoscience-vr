using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manage the interaction from to game to the network player
/// </summary>
public class GameManager : MonoBehaviour
{
    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    GameObject labyrinthRoom;

    [SerializeField]
    GameObject lobby;//should probably manage it's self or just not exist

    [SerializeField]
    ScriptableControler controls;

    [SerializeField]
    GameLabyrinth labyrinth;

    [SerializeField]
    ScriptableBoolean isEndReached;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//Tablet use only

        currentGameState.valueChangedEvent += OnGameStateChanged;

        if (isEndReached != null)
        {
            isEndReached.valueChangedEvent += OnEndReached;
        }
    }

    //Game manager could be remove and instead add a status preparing for game and have the module that need it listen to status
    public void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.PLAYING_TUTORIAL)
        {
            labyrinth.GenerateLabyrinthVisual();

            if (controls != null)
            {
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

            //Activate algorithm for tablet
            if (controls != null)
            {
                controls.isControlsEnabled = true;
            }
        }
        else if (currentGameState.value == Constants.PLAYING)
        {
            labyrinth.GenerateLabyrinthVisual();

            if (controls != null)
            {
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

            //Activate algorithm for tablet
            if (controls != null)
            {
                controls.isControlsEnabled = true;
            }
        }
        else if (currentGameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {


        }
    }

    void OnEndReached()
    {
        if(isEndReached.value)
        {
            if(currentGameState.value == Constants.PLAYING_TUTORIAL)
            {
                controls.isControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();

                isEndReached.value = false;
                controls.isControlsEnabled = true;
            }
            else
            {
                controls.isControlsEnabled = false;
                controls.StopAllMovement();


                labyrinth.DestroyLabyrinth();
                labyrinthRoom.transform.position = new Vector3(0, 0, 0);

                if (lobby != null)
                {
                    lobby.SetActive(true);
                }

                controls.ResetPositionAndRotation();

                isEndReached.value = false;

                currentGameState.value = Constants.WAITING_FOR_NEXT_ROUND;
            }
        }
    }

}
