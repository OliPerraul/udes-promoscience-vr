using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Team", order = 1)]
public class ScriptableTeam : ScriptableObject
{
    [SerializeField]
    int teamId;

    [SerializeField]
    string teamName;

    [SerializeField]
    Color teamColor;

    public int TeamId
    {
        get
        {
            return teamId;
        }
    }

    public string TeamName
    {
        get
        {
            return teamName;
        }
    }

    public Color TeamColor
    {
        get
        {
            return teamColor;
        }
    }
}

