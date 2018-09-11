using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This class manage the interaction from to game to the network player
/// </summary>
public class GameManager : MonoBehaviour
{
    //All the logic of the gameManager should be move to their repective owner, 

    //GameState
    ScriptableInteger connectionPhase;
    ScriptableInteger lobbbyPhase;
    ScriptableInteger tutorialPhase;
    ScriptableInteger gamePhase;
    ScriptableInteger intermissionPhase;


    //Might not need to be static if using sciptable object to share data
    public static GameManager instance;

    public Player localPlayer;

    ScriptableInteger currentGameState;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
            Screen.sleepTimeout = SleepTimeout.NeverSleep;//Still need testing
            //register to game status change
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetReadyStatus()
    {
        if(localPlayer != null)
        {
            localPlayer.CmdSetPlayerStatus(Constants.READY_STATUS_ID);
            UITextManager.instance.SetText("Ready!");//Should be moved to logic in their respective owner
            UITextManager.instance.HideReadyButton();//Should be moved to logic in their respective owner
        }
    }

    public void OnGameStateChanged()
    {
        if(currentGameState.value == connectionPhase.value)
        {

        }
        else if(currentGameState.value == tutorialPhase.value)
        {

        }
        else if (currentGameState.value == lobbbyPhase.value)
        {

        }
        else if (currentGameState.value == gamePhase.value)
        {

        }
        else if (currentGameState.value == intermissionPhase.value)
        {

        }
    }

}
