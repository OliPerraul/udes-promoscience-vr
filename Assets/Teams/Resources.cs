using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UdeS.Promoscience.Teams
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/TeamList", order = 1)]
    public class Resources : ScriptableObject
    {
        [FormerlySerializedAs("teamList")]
        [SerializeField]
        List<ScriptableTeam> teams;// = new List<ScriptableTeam>();

        public static Resources Instance;

        public void OnEnable()
        {
            Instance = this;
        }

        public IEnumerable<ScriptableTeam> Teams
        {
            get
            {
                return teams;
            }
        }

        int counter = 0;

        public ScriptableTeam GetUnusedScriptableTeam()
        {
            //Add check so that the team id is not used in playerinformation list instead of count
            ScriptableTeam scriptableTeam = teams[counter % teams.Count];
            counter++;
            return scriptableTeam;
        }

        public ScriptableTeam GetScriptableTeamWithId(int id)
        {
            if (id >= 0 && id < teams.Count)//Should add custom editor that sort list by id for safety mesure
            {
                return teams[id];
            }
            else
            {
                return null;
            }
        }
    }
}
