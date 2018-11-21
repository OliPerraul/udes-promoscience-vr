using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/TeamList", order = 1)]
public class ScriptableTeamList : ScriptableObject
{
    [SerializeField]
    List<ScriptableTeam> teamList = new List<ScriptableTeam>();

    int counter = 0;

    public ScriptableTeam GetUnusedScriptableTeam()
    {
        //Add check so that the team id is not used in playerinformation list instead of count
        ScriptableTeam scriptableTeam = teamList[counter % teamList.Count];
        counter++;
        return scriptableTeam;
    }

    public ScriptableTeam GetScriptableTeamWithId(int id)
    {
        if(id >= 0 && id < teamList.Count)//Should add custom editor that sort list by id for safety mesure
        {
            return teamList[id];
        }
        else
        {
            return null;
        }
    }
}

