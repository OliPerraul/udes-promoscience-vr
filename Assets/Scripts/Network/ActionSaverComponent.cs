using System.Collections;
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
            int teamId = 0;
            int teamName = 0;
            int teamColor = 0;
            int courseId = 0;
            int labyrithId = 0;
            int algorithmId = 0;
            int eventType = 0;
            System.DateTime now =  System.DateTime.Now;

            SQLiteUtilities.InsertPlayerAction(teamId, teamName, teamColor, courseId, labyrithId, algorithmId, eventType, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss"));
            Debug.Log("Save Action for player : " + player.deviceName);
        }
    }
#endif
}
