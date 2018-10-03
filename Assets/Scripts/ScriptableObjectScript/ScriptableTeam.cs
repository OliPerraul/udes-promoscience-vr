﻿using System;
using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "Data/Team", order = 1)]
public class ScriptableTeam : ScriptableObject
{
    public string teamName;
    public Color teamColor;
}
