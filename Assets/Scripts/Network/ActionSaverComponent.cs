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
            player.sPlayerActionChangedEvent += SaveActionInDatabase;
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
        if (player.sPlayerStatus == Constants.PLAYING)
        {
            int teamId = player.sTeamId;
            string teamName = player.sTeamName;
            string teamColor = player.sTeamColor.ToString();
            int courseId = player.sCourseId;
            int labyrithId = player.sLabyrinthId;
            int algorithmId = player.sAlgorithmId;

            System.DateTime now =  System.DateTime.Now;

            SQLiteUtilities.InsertPlayerAction(teamId, teamName, teamColor, courseId, labyrithId, algorithmId, player.sPlayerAction, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss"));
        }
    }
#endif
}
