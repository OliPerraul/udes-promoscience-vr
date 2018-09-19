﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionSaverComponent : NetworkBehaviour
{
    [SerializeField]
    Player player;

    void Start ()
    {
        if (isServer)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            player.playerActionChangedEvent += SaveActionInDatabase;
#endif
        }
        else
        {
            Destroy(this);
        }
    }

#if UNITY_EDITOR || UNITY_STANDALONE_WIN

    void SaveActionInDatabase()
    {
        if (player.playerStatus == Constants.PLAYING)
        {
            //to do
            int teamId = 1;
            int teamName = 1;
            int teamColor = 1;
            int courseId = 1;
            int labyrithId = 1;
            int algorithmId = 1;

            System.DateTime now =  System.DateTime.Now;

            SQLiteUtilities.InsertPlayerAction(teamId, teamName, teamColor, courseId, labyrithId, algorithmId, player.playerAction, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss"));
            Debug.Log("Save Action for player : " + player.deviceName);
        }
    }
#endif
}
