using Cirrus.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace UdeS.Promoscience.Teams
{

    [CreateAssetMenu(fileName = "Data", menuName = "Data/TeamList", order = 1)]
    public class Resources : BaseResources<Resources>
    {
        [FormerlySerializedAs("teamList")]
        [SerializeField]
        List<TeamResource> teams;// = new List<ScriptableTeam>();

        public IEnumerable<TeamResource> Teams
        {
            get
            {
                return teams;
            }
        }

        int counter = 0;

        public TeamResource GetUnusedScriptableTeam()
        {
            //Add check so that the team id is not used in playerinformation list instead of count
            TeamResource scriptableTeam = teams[counter % teams.Count];
            counter++;
            return scriptableTeam;
        }

        public TeamResource GetScriptableTeamWithId(int id)
        {
            if (id >= 0 && id < teams.Count)//Should add custom editor that sort list by id for safety mesure
            {
                return teams[id];
            }
            else
            {
                // FIX workaroud TODO 
                return teams[id.Mod(teams.Count)];
                //return null;
            }
        }
    }
}
