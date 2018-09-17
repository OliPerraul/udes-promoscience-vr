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
    //Controls
    //Algorithm for tablet
    //MessageClient or message server

    [SerializeField]
    LabyrinthVisual labyrinth;

    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;//Still need testing

        currentGameState.valueChangedEvent += OnGameStateChanged;
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

            //Activate looping tutorial option
            //Activate algorithm for tablet
            //Send ready to partner?? on response unlock controls?
            //Unlock controls
        }
        else if (currentGameState.value == Constants.PLAYING)
        {
            labyrinth.GenerateLabyrinthVisual();
            //Play animation room collapsing or whatever
            labyrinthRoom.GetComponent<Animation>().Play();//Could be moved to a repective module
            lobby.SetActive(false);
            //Deactivate looping tutorial option so that at the end, go to waiting scene
            //Activate algorithm for tablet
            //Send ready to partner?? on response unlock controls?
            //Unlock controls
        }
        else if (currentGameState.value == Constants.WAITING_FOR_NEXT_ROUND)
        {

        }
    }

}
