using System;
using System.Collections;
using UnityEngine;

namespace UdeS.Promoscience.ScriptableObjects
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/Team", order = 1)]
    public class ScriptableTeam : ScriptableObject
    {
        [SerializeField]
        int teamId;

        [SerializeField]
        ScriptableLocalizeString teamName;

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
