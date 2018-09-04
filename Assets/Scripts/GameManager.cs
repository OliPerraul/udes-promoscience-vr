using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    //Might not need to be static if using sciptable object to share data
    public static GameManager instance;

    public Player localPlayer;

    void Start()
    {
        if (instance == null)
        {
            instance = this;
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
            localPlayer.CmdSetPlayerStatus(1);
            UITextManager.instance.SetText("Ready!");
            UITextManager.instance.HideReadyButton();
        }
    }

}
