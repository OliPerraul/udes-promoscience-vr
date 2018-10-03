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

    public ScriptableTeam GetScriptableTeam()
    {
        ScriptableTeam scriptableTeam = teamList[counter % teamList.Count];
        counter++;
        return scriptableTeam;
    }
}

