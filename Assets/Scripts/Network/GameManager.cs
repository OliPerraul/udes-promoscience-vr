using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manage the interaction from to game to the network player
/// </summary>
public class GameManager : MonoBehaviour
{
    //All the logic of the gameManager should be move to their repective owner, 
    Player localPlayer;

    [SerializeField]
    ScriptableInteger currentGameState;

    [SerializeField]
    GameObject labyrinthRoom;

    [SerializeField]
    GameObject lobby;//should probably manage it's self or just not exist

    //GameComponents
    //Algorithm for tablet
    //MessageClient or message server

    [SerializeField]
    SlideMovementHeadLookControls controls;

    [SerializeField]
    LabyrinthVisual labyrinth;

    [SerializeField]
    ScriptableLabyrinth labyrinthData;

    [SerializeField]
    ScriptableBoolean isEndReached;

    [SerializeField]
    Transform cameraTransform;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//Tablet use only

        currentGameState.valueChangedEvent += OnGameStateChanged;
        isEndReached.valueChangedEvent += OnEndReached;
    }

    //Game manager could be remove and instead add a status preparing for game and have the module that need it listen to status
    public void OnGameStateChanged()
    {
        if (currentGameState.value == Constants.PLAYING_TUTORIAL)
        {
            labyrinth.GenerateLabyrinthVisual();
            //Play animation room collapsing or whatever

            labyrinthRoom.GetComponent<Animation>().Play();//Could be moved to a repective module
            lobby.SetActive(false);

            //Activate algorithm for tablet
            if(controls != null)
            {
                controls.SetMovementActive(true);
            }
        }
        else if (currentGameState.value == Constants.PLAYING)
        {
            labyrinth.GenerateLabyrinthVisual();
            //Play animation room collapsing or whatever
            labyrinthRoom.GetComponent<Animation>().Play();//Could be moved to a repective module
            lobby.SetActive(false);
            //Deactivate looping tutorial option so that at the end, go to waiting scene
            //Activate algorithm for tablet
            if (controls != null)
            {
                controls.SetMovementActive(true);
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
                controls.SetMovementActive(false);
                controls.StopAllMovement();
                Vector2Int startPos = labyrinthData.GetLabyrithStartPosition();
                cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
                isEndReached.value = false;
                //Send pos to tablet
                controls.SetMovementActive(true);
            }
            else
            {
                controls.SetMovementActive(false);
                controls.StopAllMovement();

                labyrinth.DestroyLabyrinth();
                //Not tested yet
                labyrinthRoom.GetComponent<Animation>().Play();//Could be moved to a repective module
                labyrinthRoom.GetComponent<Animation>().Stop();//Could be moved to a repective module
                lobby.SetActive(true);

                Vector2Int startPos = labyrinthData.GetLabyrithStartPosition();
                cameraTransform.position = new Vector3(0, cameraTransform.position.y, 0);
                isEndReached.value = false;
                //Change for waiting scene (Ui job) 
                //Sent message to tablet
            }
        }
    }

}
