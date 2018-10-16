﻿using System.Collections;
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
    ScriptableInteger gameState;

    [SerializeField]
    ScriptableBoolean isEndReached;

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

        if (isEndReached != null)
        {
            isEndReached.valueChangedEvent += OnEndReached;
        }
    }

    //Game manager could be remove and instead add a status preparing for game and have the module that need it listen to status
    public void OnGameStateChanged()
    {
        if (gameState.value == Constants.PLAYING_TUTORIAL)
        {
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

            if (controls != null)
            {
                controls.isControlsEnabled = true;
            }
        }
        else if (gameState.value == Constants.PLAYING)
        {
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

            if (controls != null)
            {
                controls.isControlsEnabled = true;
            }
        }
        else if (gameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {


        }
    }

    void OnEndReached()
    {
        if(isEndReached.value)
        {
            if(gameState.value == Constants.PLAYING_TUTORIAL)
            {
                controls.isControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                isEndReached.value = false;

                labyrinthRoom.transform.position = new Vector3(0, 0, 0);

                gameState.value = Constants.TUTORIAL_LABYRITH_READY;
            }
            else
            {
                controls.isControlsEnabled = false;
                controls.StopAllMovement();
                controls.ResetPositionAndRotation();
                isEndReached.value = false;

                labyrinth.DestroyLabyrinth();
                labyrinthRoom.transform.position = new Vector3(0, 0, 0);

                if (lobby != null)
                {
                    lobby.SetActive(true);
                }

                gameState.value = Constants.WAITING_FOR_NEXT_ROUND;
            }
        }
    }

}
