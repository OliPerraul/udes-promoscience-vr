using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

using UdeS.Promoscience.ScriptableObjects;
using UdeS.Promoscience.Utils;
using UdeS.Promoscience;

namespace UdeS.Promoscience.Network
{
    public class ActionSaverComponent : NetworkBehaviour
    {
        [SerializeField]
        ScriptableTeamList teamList;

        [SerializeField]
        Player player;

        void Start()
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
            if (player.ServerPlayerGameState == ClientGameState.Playing ||
                player.ServerPlayerGameState == ClientGameState.PlayingTutorial )
            {
                int teamId = player.ServerTeamId;
                ScriptableTeam team = teamList.GetScriptableTeamWithId(player.ServerTeamId);
                string teamName = team.TeamName;
                string teamColor = team.TeamColor.ToString();
                int courseId = player.ServerCourseId;
                int labyrithId = player.serverLabyrinthId;
                int algorithmId = (int)player.serverAlgorithm;

                SQLiteUtilities.InsertPlayerAction(
                    teamId, 
                    teamName, 
                    teamColor, 
                    courseId, 
                    labyrithId, 
                    algorithmId, 
                    (int)player.ServerPlayerGameAction, 
                    player.ServerPlayerGameActionDateTimeString, 
                    player.ServerPlayerGameActionValue);
            }
        }
#endif
    }
}
