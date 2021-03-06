﻿using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.Teams
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Team", order = 1)]
    public class TeamResource : ScriptableObject
    {
        [SerializeField]
        int teamId;

        [SerializeField]
        LocalizeInlineString teamName = new LocalizeInlineString("Team", "Equipe");

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
                return teamName.Value;
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
}
