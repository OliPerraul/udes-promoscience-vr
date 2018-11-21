﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ActionSaverComponent : NetworkBehaviour
{
    [SerializeField]
    ScriptableTeamList teamList;

    [SerializeField]
    Player player;

    void Start ()
    {
        if (isServer)
        {
#if UNITY_EDITOR || UNITY_STANDALONE_WIN
            player.serverPlayerActionChangedEvent += SaveActionInDatabase;
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
        if (player.ServerPlayerGameState == ClientGameState.Playing)
        {
            int teamId = player.ServerTeamId;
            ScriptableTeam team = teamList.GetScriptableTeamWithId(player.ServerTeamInformationId);
            string teamName = team.TeamName;
            string teamColor = team.TeamColor.ToString();
            int courseId = player.ServerCourseId;
            int labyrithId = player.serverLabyrinthId;
            int algorithmId = (int) player.serverAlgorithm;

            System.DateTime now =  System.DateTime.Now;

            SQLiteUtilities.InsertPlayerAction(teamId, teamName, teamColor, courseId, labyrithId, algorithmId, (int) player.ServerPlayerAction, now.ToString("yyyy-MM-dd"), now.ToString("HH:mm:ss"));
        }
    }
#endif
}
